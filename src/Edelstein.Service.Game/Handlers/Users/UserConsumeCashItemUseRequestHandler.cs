using System.Threading.Tasks;
using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Gameplay.Extensions;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserConsumeCashItemUseRequestHandler : AbstractUseItemHandler<ItemTemplate>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task<bool> Handle(
            FieldUser user,
            ItemSlot item,
            ItemTemplate template,
            RecvPacketOperations operation,
            IPacketDecoder packet)
        {
            var type = template.ID / 10000;

            switch (type)
            {
                case 506:
                    if (template.ID == 5062000)
                        return await UseMiracleCube(user, packet);
                    break;
                default:
                    Logger.Warn($"Unhandled consume cash item request type: {type}");
                    break;
            }

            return false;
        }

        private async Task<bool> UseMiracleCube(FieldUser user, IPacketDecoder packet)
        {
            var targetSlot = packet.DecodeShort();
            var targetItem = user.Character.Inventories[ItemInventoryType.Equip][targetSlot];

            if (!(targetItem is ItemSlotEquip target)) return false;
            if (target.Grade < 5) return false;
            if (user.Character.AvailableSlotsFor(ItemInventoryType.Consume) == 0)
            {
                using var p = new OutPacket(SendPacketOperations.UserItemUnreleaseEffect);
                p.EncodeInt(user.ID);
                p.EncodeBool(false);
                await user.SendPacket(p);
                return false;
            }

            await user.UnreleaseItemOption(target);

            using var p2 = new OutPacket(SendPacketOperations.UserItemUnreleaseEffect);

            p2.EncodeInt(user.ID);
            p2.EncodeBool(true);

            await user.SendPacket(p2);
            await user.ModifyInventory(i => i.Add(user.Service.TemplateManager.Get<ItemTemplate>(2430112)));
            return true;
        }
    }
}