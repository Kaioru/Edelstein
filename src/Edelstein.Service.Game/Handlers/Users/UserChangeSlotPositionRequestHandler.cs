using System.Threading.Tasks;
using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
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
                    var item = user.Character.Inventories[type][from];

                    if (!ItemConstants.IsTreatSingly(item.TemplateID))
                    {
                        if (!(item is ItemSlotBundle bundle)) return;
                        if (bundle.Number < number) return;

                        i[type].Take(@from, number);
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