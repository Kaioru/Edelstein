using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Summoned;

namespace Edelstein.Service.Game.Services.Handlers.Summoned
{
    public class SummonedMoveHandler : AbstractFieldSummonedHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldSummoned summoned)
        {
            using (var p = new Packet(SendPacketOperations.SummonedMove))
            {
                p.Encode<int>(summoned.Owner.ID);
                p.Encode<int>(summoned.ID);

                summoned.Move(packet).Encode(p);

                await summoned.Field.BroadcastPacket(summoned.Owner, p);
            }
        }
    }
}