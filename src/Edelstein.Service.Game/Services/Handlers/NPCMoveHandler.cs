using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.NPC;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class NPCMoveHandler : AbstractFieldNPCHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldNPC npc)
        {
            using var p = new Packet(SendPacketOperations.NpcMove);
            p.Encode<int>(npc.ID);
            p.Encode<byte>(packet.Decode<byte>()); // TODO: validate acts
            p.Encode<byte>(packet.Decode<byte>());

            if (npc.Template.Move)
            {
                npc.Move(packet).Encode(p);
            }

            await npc.Field.BroadcastPacket(p);
        }
    }
}