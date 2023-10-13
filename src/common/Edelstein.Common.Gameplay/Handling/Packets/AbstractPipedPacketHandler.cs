using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Packets;

public abstract class AbstractPipedPacketHandler<TStageUser, TMessage> : 
    IPacketSerializer<TStageUser, TMessage>, 
    IPacketHandler<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPipeline<TMessage> _pipeline;

    protected AbstractPipedPacketHandler(IPipeline<TMessage> pipeline) => _pipeline = pipeline;
    
    public abstract short Operation { get; }

    public abstract bool Check(TStageUser user);
    public Task Handle(TStageUser user, IPacketReader reader)
    {
        var message = Serialize(user, reader);
        return message != null ? _pipeline.Process(message) : Task.CompletedTask;
    }

    public abstract TMessage? Serialize(TStageUser user, IPacketReader reader);
}
