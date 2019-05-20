using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserGatherItemRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = user.Character.Inventories[inventoryType].Items
                .Where(kv => kv.Key > 0)
                .OrderBy(kv => kv.Key)
                .ToList();
            short position = 1;

            await user.ModifyInventory(i =>
            {
                inventoryCopy.ForEach(kv => i.Remove(kv.Value));
                inventoryCopy.ForEach(kv => i.Set(position++, kv.Value));
            }, true);

            using (var p = new Packet(SendPacketOperations.GatherItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                await user.SendPacket(p);
            }
        }
    }
}