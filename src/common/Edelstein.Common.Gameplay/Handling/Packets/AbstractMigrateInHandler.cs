using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Packets;

public abstract class AbstractMigrateInHandler<TStageUser> : 
    AbstractPipedPacketHandler<TStageUser, UserOnPacketMigrateIn<TStageUser>>, 
    IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    protected AbstractMigrateInHandler(IPipeline<UserOnPacketMigrateIn<TStageUser>> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.MigrateIn;

    public override bool Check(TStageUser user) =>
        !user.IsMigrating &&
        user.Account == null &&
        user.AccountWorld == null &&
        user.Character == null;
    
    public override UserOnPacketMigrateIn<TStageUser> Serialize(TStageUser user, IPacketReader reader) 
        => new(
            user,
            reader.ReadInt(),
            reader.Skip(18).ReadLong()
        );
}
