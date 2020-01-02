using System;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Utils;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserChangeSlotPositionRequestHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            packet.Decode<int>();
            var type = (ItemInventoryType) packet.Decode<byte>();
            var from = packet.Decode<short>();
            var to = packet.Decode<short>();
            var number = packet.Decode<short>();

            if (to == 0)
            {
                await user.ModifyInventory(i =>
                {
                    var item = user.Character.Inventories[type].Items[from];

                    if (!ItemConstants.IsTreatSingly(item.TemplateID))
                    {
                        if (!(item is ItemSlotBundle bundle)) return;
                        if (bundle.Number < number) return;

                        item = i[type].Take(from, number);
                    }
                    else i[type].Remove(from);

                    // TODO: drops
                }, true);
                return;
            }

            // TODO: equippable checks
            await user.ModifyInventory(i => i[type].Move(from, to), true);
        }
    }
}