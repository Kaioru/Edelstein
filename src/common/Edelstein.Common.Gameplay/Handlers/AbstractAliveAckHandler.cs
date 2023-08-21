using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handlers;

public abstract class AbstractAliveAckHandler<TStageUser>: IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<UserOnPacketAliveAck<TStageUser>> _pipeline;

    protected AbstractAliveAckHandler(IPipeline<UserOnPacketAliveAck<TStageUser>> pipeline) => _pipeline = pipeline;

    public short Operation => (short)PacketRecvOperations.AliveAck;

    public bool Check(TStageUser user) => user.Stage != null;

    public Task Handle(TStageUser user, IPacketReader reader) =>
        _pipeline.Process(new UserOnPacketAliveAck<TStageUser>(user, DateTime.UtcNow));
}
