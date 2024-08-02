using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipeline<TMessage>
{
    void Add(int priority, IPipe<TMessage> pipe);
    void Add(IPipe<TMessage> pipe);
    void Remove(IPipe<TMessage> pipe);

    Task<IPipelineContext> Process(TMessage message);
}
