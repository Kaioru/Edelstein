using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Contracts.Pipelines;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;

public class NPCMoveHandler : AbstractFieldNPCHandler
{
    private readonly IPipeline<INPCMove> _pipeline;

    public NPCMoveHandler(IPipeline<INPCMove> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.NpcMove;

    protected override Task Handle(IFieldUser user, IFieldNPC npc, IPacketReader reader)
    {
        var message = new NPCMove(user, npc, reader.Read(new NPCMovePath(npc.Template.Move)));

        return _pipeline.Process(message);
    }
}
