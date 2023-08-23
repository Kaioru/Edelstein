using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

internal class PipelinePart<TMessage>
{
    public PipelinePart(int priority, bool isDefaultAction, IPipelinePlug<TMessage> plug)
    {
        Priority = priority;
        IsDefaultAction = isDefaultAction;
        Plug = plug;
    }
    
    public int Priority { get; }
    public bool IsDefaultAction { get; }
    public IPipelinePlug<TMessage> Plug { get; }
}
