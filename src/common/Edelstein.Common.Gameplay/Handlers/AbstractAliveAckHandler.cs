using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handlers;

public abstract class AbstractAliveAckHandler<TStageUser> : 
    AbstractPipedPacketHandler<TStageUser, UserOnPacketAliveAck<TStageUser>>, 
    IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    protected AbstractAliveAckHandler(IPipeline<UserOnPacketAliveAck<TStageUser>?> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.AliveAck;

    public override bool Check(TStageUser user) => user.Stage != null;
    
    public override UserOnPacketAliveAck<TStageUser> Serialize(TStageUser user, IPacketReader reader) 
        => new(user, DateTime.UtcNow);
}
