using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Pipelines;
using NPCMoveRecv = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv.NPCMove;
using NPCMoveSend = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send.NPCMove;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketNPCMove : AbstractUserOnPacketInField<NPCMoveRecv>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<NPCMoveRecv> message)
    {
        var obj = message.User.Field?
            .GetPool(FieldObjectType.NPC)?
            .GetObject(message.Packet.ObjectID);

        if (obj is not IFieldNPC npc || npc.Controller != message.User) return;
        if (npc.Template.Move && message.Packet.Path != null)
        {
            var path = new FieldNPCMovePath();

            path.Apply(message.Packet.Path);
            await npc.UpdatePosition(path);
        }

        if (npc.FieldSplit != null)
            await npc.FieldSplit.Dispatch(
                new NPCMoveSend
                {
                    ObjectID = message.Packet.ObjectID,
                    Action = message.Packet.Action,
                    ChatIdx = message.Packet.ChatIdx,
                    Path = npc.Template.Move ? message.Packet.Path : null
                }
            );
    }
}
