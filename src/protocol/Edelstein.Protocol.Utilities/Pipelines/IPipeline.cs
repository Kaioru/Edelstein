namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipeline<TMessage>
{
    void Add(int priority, IPipelinePlug<TMessage> plug);
    void Add(IPipelinePlug<TMessage> plug);
    void Remove(IPipelinePlug<TMessage> plug);

    Task<IPipelineContext> Process(TMessage message);
}
