using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserItemReleaseRequestHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();

            var releaseType = packet.DecodeShort();
            var targetSlot = packet.DecodeShort();
            var targetItem = user.Character.Inventories[ItemInventoryType.Equip][targetSlot];

            if (!(targetItem is ItemSlotEquip target)) return;

            await user.ReleaseItemOption(target);

            using var p = new OutPacket(SendPacketOperations.UserItemReleaseEffect);

            p.EncodeInt(user.ID);
            p.EncodeShort(targetSlot);

            await user.SendPacket(p);
            await user.ModifyInventory(exclRequest: true);
        }
    }
}