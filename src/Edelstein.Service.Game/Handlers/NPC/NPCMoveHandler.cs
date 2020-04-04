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
        protected override async Task Handle(FieldNPC npc, RecvPacketOperations operation, IPacketDecoder packet)
        {
            using var p = new OutPacket(SendPacketOperations.NpcMove);

            p.EncodeInt(npc.ID);
            p.EncodeByte(packet.DecodeByte()); // TODO: validate acts
            p.EncodeByte(packet.DecodeByte());

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