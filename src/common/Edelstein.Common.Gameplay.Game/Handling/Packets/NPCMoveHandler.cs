using Edelstein.Common.Gameplay.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class NPCMoveHandler : AbstractPipedFieldNPCHandler<FieldOnPacketNPCMove>
{
    public NPCMoveHandler(IPipeline<FieldOnPacketNPCMove> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.NpcMove;

    protected override FieldOnPacketNPCMove? Serialize(IFieldUser user, IFieldNPC npc, IPacketReader reader)
        => new(user, npc, reader.Read(new FieldNPCMovePath(npc.Template.Move)));
}
