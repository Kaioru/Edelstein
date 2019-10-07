using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Dragon;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class DragonMoveHandler : AbstractFieldDragonHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldDragon dragon)
        {
            using var p = new Packet(SendPacketOperations.DragonMove);
            p.Encode<int>(dragon.Owner.ID);
            dragon.Move(packet).Encode(p);

            await dragon.Field.BroadcastPacket(dragon.Owner, p);
        }
    }
}