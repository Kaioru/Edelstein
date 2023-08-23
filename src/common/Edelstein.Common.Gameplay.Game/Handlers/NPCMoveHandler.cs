using Edelstein.Common.Gameplay.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class NPCMoveHandler : AbstractFieldNPCHandler
{
    private readonly IPipeline<FieldOnPacketNPCMove> _pipeline;

    public NPCMoveHandler(IPipeline<FieldOnPacketNPCMove> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.NpcMove;

    protected override Task Handle(IFieldUser user, IFieldNPC npc, IPacketReader reader)
    {
        var message = new FieldOnPacketNPCMove(user, npc, reader.Read(new FieldNPCMovePath(npc.Template.Move)));

        return _pipeline.Process(message);
    }
}
