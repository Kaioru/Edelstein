using System;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Items;
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
            var targetItem = user.Character.Inventories[ItemInventoryType.Equip].Items[targetSlot];

            if (!(targetItem is ItemSlotEquip target)) return;

            // TODO: magnifying type checking, meso checks, removal of item and mesos

            target.Grade = (byte) (target.Grade + 4);
            target.Option1 = Math.Abs(target.Option1);
            target.Option2 = Math.Abs(target.Option2);
            target.Option3 = Math.Abs(target.Option3);

            using var p = new OutPacket(SendPacketOperations.UserItemReleaseEffect);

            p.EncodeInt(user.ID);
            p.EncodeShort(targetSlot);

            await user.SendPacket(p);
            await user.ModifyInventory(i => i.Update(target), true);
        }
    }
}