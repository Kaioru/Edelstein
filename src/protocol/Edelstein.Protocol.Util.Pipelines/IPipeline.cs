namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipeline<TMessage>
{
    void Add(int priority, IPipelinePlug<TMessage> plug);
    void Remove(IPipelinePlug<TMessage> plug);

    Task Process(TMessage message);
}
