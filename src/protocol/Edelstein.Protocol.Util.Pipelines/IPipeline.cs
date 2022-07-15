namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipeline<TMessage>
{
    void Add(int priority, IPipelineAction<TMessage> action);
    void Remove(IPipelineAction<TMessage> action);

    Task Process(TMessage message);
}
