using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserChangeSlotPositionRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
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

                    var drop = new ItemFieldDrop(item)
                    {
                        Position = user.Position,
                        DateExpire = DateTime.Now.AddMinutes(3)
                    };
                    user.Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, user));
                }, true);
                return;
            }

            // TODO: equippable checks
            await user.ModifyInventory(i => i[type].Move(from, to), true);
        }
    }
}