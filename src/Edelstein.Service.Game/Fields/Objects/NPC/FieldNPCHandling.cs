using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.NPC
{
    public partial class FieldNPC
    {
        private async Task OnNPCMove(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.NpcMove))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(packet.Decode<byte>()); // TODO: validate acts
                p.Encode<byte>(packet.Decode<byte>());

                if (Template.Move)
                {
                    Move(packet).Encode(p);
                }

                await Field.BroadcastPacket(p);
            }
        }
    }
}