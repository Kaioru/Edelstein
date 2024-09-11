using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipeline<TMessage> : IPipework<TMessage>
{
    Task<IPipelineContext> Process(TMessage message);
}
