using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Messages;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Handlers;

public abstract class AbstractAliveAckHandler<TStageUser> : IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<ISocketOnAliveAck<TStageUser>> _pipeline;

    public AbstractAliveAckHandler(IPipeline<ISocketOnAliveAck<TStageUser>> pipeline) => _pipeline = pipeline;

    public short Operation => (short)PacketRecvOperations.AliveAck;

    public bool Check(TStageUser user) => user.Stage != null;

    public Task Handle(TStageUser user, IPacketReader reader) =>
        _pipeline.Process(new SocketOnAliveAck<TStageUser>(user, DateTime.UtcNow));
}
