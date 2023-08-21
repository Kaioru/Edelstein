using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handlers;

public class AbstractMigrateInHandler<TStageUser> : IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<UserOnPacketMigrateIn<TStageUser>> _pipeline;

    protected AbstractMigrateInHandler(IPipeline<UserOnPacketMigrateIn<TStageUser>> pipeline) => _pipeline = pipeline;

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
        var message = new UserOnPacketMigrateIn<TStageUser>(
            user,
            character,
            key
        );

        return _pipeline.Process(message);
    }
}
