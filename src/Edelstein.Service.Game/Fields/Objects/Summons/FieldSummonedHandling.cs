using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.Summons
{
    public partial class FieldSummoned
    {
        private async Task OnSummonedMove(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.SummonedMove))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<int>(ID);
                
                Move(packet).Encode(p);
                
                await Field.BroadcastPacket(Owner, p);
            }
        }
    }
}