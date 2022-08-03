using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Messages;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Handlers;

public abstract class AbstractMigrateInHandler<TStageUser> : IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<ISocketOnMigrateIn<TStageUser>> _pipeline;

    protected AbstractMigrateInHandler(IPipeline<ISocketOnMigrateIn<TStageUser>> pipeline) => _pipeline = pipeline;

    public short Operation => (short)PacketRecvOperations.MigrateIn;

    public bool Check(TStageUser user) =>
        !user.IsMigrating &&
        user.Account == null &&
        user.AccountWorld == null &&
        user.Character == null;

    public Task Handle(TStageUser user, IPacketReader reader)
    {
        var character = reader.ReadInt();
        _ = reader.ReadBytes(18);
        var key = reader.ReadLong();
        var message = new SocketOnMigrateIn<TStageUser>(
            user,
            character,
            key
        );

        return _pipeline.Process(message);
    }
}
