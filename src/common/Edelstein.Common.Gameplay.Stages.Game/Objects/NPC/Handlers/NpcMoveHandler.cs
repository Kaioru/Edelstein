using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;

public class NpcMoveHandler : AbstractFieldNPCHandler
{
    public override short Operation => (short)PacketRecvOperations.NpcMove;

    protected override Task Handle(IFieldUser user, IFieldNPC npc, IPacketReader reader)
    {
        var path = reader.Read(new NPCMovePath(npc.Template.Move));

        return npc.Move(path);
    }
}
