using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.NPC;

namespace Edelstein.Service.Game.Handlers.NPC
{
    public class NPCMoveHandler : AbstractFieldNPCHandler
    {
        protected override async Task Handle(FieldNPC npc, RecvPacketOperations operation, IPacket packet)
        {
            using var p = new Packet(SendPacketOperations.NpcMove);

            p.Encode<int>(npc.ID);
            p.Encode<byte>(packet.Decode<byte>()); // TODO: validate acts
            p.Encode<byte>(packet.Decode<byte>());

            if (npc.Template.Move)
            {
                var path = new MovePath(packet);

                path.Encode(p);
                await npc.Move(path);
            }

            await npc.BroadcastPacket(p);
        }
    }
}