using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Templates.Items.ItemOption;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;
using MoreLinq.Extensions;

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
            var targetItem = user.Character.Inventories[ItemInventoryType.Equip].Items[targetSlot];
            var template = user.Service.TemplateManager.Get<ItemTemplate>(targetItem.TemplateID);

            if (!(targetItem is ItemSlotEquip target)) return false;
            if (!(template is ItemEquipTemplate targetTemplate)) return false;
            if (target.Grade <= 0) return false;
            if (user.Character.AvailableSlotsFor(ItemInventoryType.Consume) == 0)
            {
                using var p = new OutPacket(SendPacketOperations.UserItemUnreleaseEffect);
                p.EncodeInt(user.ID);
                p.EncodeBool(false);
                await user.SendPacket(p);
                return false;
            }

            var random = new Random();
            var grade = (ItemOptionGrade) (target.Grade - 4);

            if (grade == ItemOptionGrade.Rare && random.Next(100) <= 12 ||
                grade == ItemOptionGrade.Epic && random.Next(100) <= 4)
                grade++;

            var gradeMax = grade;
            var gradeMin = grade - 1;

            gradeMax = (ItemOptionGrade) Math.Min(Math.Max((int) gradeMax, 1), 3);
            gradeMin = (ItemOptionGrade) Math.Min(Math.Max((int) gradeMin, 0), 2);

            var options = user.Service.TemplateManager
                .GetAll<ItemOptionTemplate>()
                .Where(o => targetTemplate.ReqLevel >= o.ReqLevel)
                .ToImmutableList();
            var maxOptions = options
                .Where(o => o.Grade == gradeMax)
                .ToImmutableList();
            var minOptions = options
                .Where(o => o.Grade == gradeMin)
                .ToImmutableList();

            // TODO: filter optionType

            target.Grade = (byte) grade;
            target.Option1 = (short) (maxOptions.Shuffle().FirstOrDefault()?.ID ?? 0);
            target.Option2 = random.Next(100) <= 4
                ? (short) (maxOptions.Shuffle().FirstOrDefault()?.ID ?? 0)
                : (short) (minOptions.Shuffle().FirstOrDefault()?.ID ?? 0);
            if (target.Option3 > 0)
                target.Option3 = random.Next(100) <= 4
                    ? (short) (maxOptions.Shuffle().FirstOrDefault()?.ID ?? 0)
                    : (short) (minOptions.Shuffle().FirstOrDefault()?.ID ?? 0);

            target.Option1 = (short) -target.Option1;
            target.Option2 = (short) -target.Option2;
            target.Option3 = (short) -target.Option3;

            using var p2 = new OutPacket(SendPacketOperations.UserItemUnreleaseEffect);

            p2.EncodeInt(user.ID);
            p2.EncodeBool(true);

            await user.SendPacket(p2);
            await user.ModifyInventory(i =>
            {
                i.Add(user.Service.TemplateManager.Get<ItemTemplate>(2430112));
                i.Update(target);
            });
            return true;
        }
    }
}