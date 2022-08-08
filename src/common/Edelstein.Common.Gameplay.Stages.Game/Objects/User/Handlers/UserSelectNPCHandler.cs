using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserSelectNPCHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserSelectNPC> _pipeline;

    public UserSelectNPCHandler(IPipeline<IUserSelectNPC> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserSelectNpc;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var objID = reader.ReadInt();
        var obj = user.Field?.GetPool(FieldObjectType.NPC)?.GetObject(objID);

        if (obj is not IFieldNPC npc) return;
        if (npc.FieldSplit != null && !user.Observing.Contains(npc.FieldSplit)) return;

        var message = new UserSelectNPC(
            user,
            npc
        );

        await _pipeline.Process(message);
    }
}
