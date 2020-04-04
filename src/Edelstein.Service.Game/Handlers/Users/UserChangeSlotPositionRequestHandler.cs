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
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();
            var type = (ItemInventoryType) packet.DecodeByte();
            var from = packet.DecodeShort();
            var to = packet.DecodeShort();
            var number = packet.DecodeShort();

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