using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

internal class PipelinePart<TMessage> : IComparer<PipelinePart<TMessage>>
{
    private int Priority { get; }
    internal IPipelinePlug<TMessage> Plug { get; }
    
    public PipelinePart(int priority, IPipelinePlug<TMessage> plug)
    {
        Priority = priority;
        Plug = plug;
    }

    public int Compare(PipelinePart<TMessage>? x, PipelinePart<TMessage>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Priority.CompareTo(y.Priority);
    }
}
