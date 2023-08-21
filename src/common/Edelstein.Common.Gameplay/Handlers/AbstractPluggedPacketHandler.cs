using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handlers;

public abstract class AbstractPluggedPacketHandler<TStageUser, TMessage> : IPacketHandler<TStageUser>, IPipelinePlug<TMessage>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<TMessage> _pipeline;
    
    public abstract short Operation { get; }

    protected AbstractPluggedPacketHandler(IPipeline<TMessage> pipeline) => _pipeline = pipeline;
    
    public abstract bool Check(TStageUser user);
    protected abstract TMessage Serialize(TStageUser user, IPacketReader reader);
    public abstract Task Handle(IPipelineContext ctx, TMessage message);

    public Task Handle(TStageUser user, IPacketReader reader)
        => _pipeline.Process(Serialize(user, reader));
}
