using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

internal class PipelineStep<TMessage>(
    int priority, 
    IPipelinePlug<TMessage> plug
)
{
    public int Priority { get; } = priority;
    public IPipelinePlug<TMessage> Plug { get; } = plug;
}
