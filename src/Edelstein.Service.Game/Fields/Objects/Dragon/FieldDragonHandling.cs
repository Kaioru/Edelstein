using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.Dragon
{
    public partial class FieldDragon
    {
        private async Task OnDragonMove(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.DragonMove))
            {
                p.Encode<int>(Owner.ID);
                Move(packet).Encode(p);

                await Field.BroadcastPacket(Owner, p);
            }
        }
    }
}