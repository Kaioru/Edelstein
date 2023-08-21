using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Pipelines;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handlers;

public class AbstractAliveAckHandler<TStageUser> : AbstractPluggedPacketHandler<TStageUser, UserOnAliveAck<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    public override short Operation => (short)PacketRecvOperations.AliveAck;
    
    public AbstractAliveAckHandler(IPipeline<UserOnAliveAck<TStageUser>> pipeline) : base(pipeline)
    {
    }
    
    public override bool Check(TStageUser user) => user.Stage != null;

    protected override UserOnAliveAck<TStageUser> Serialize(TStageUser user, IPacketReader reader) 
        => new(user, DateTime.UtcNow);
    
    public override Task Handle(IPipelineContext ctx, UserOnAliveAck<TStageUser> message) 
        => Task.FromResult(message.User.Socket.LastAliveRecv = DateTime.UtcNow);
}
