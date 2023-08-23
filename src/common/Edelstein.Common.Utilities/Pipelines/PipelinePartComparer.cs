namespace Edelstein.Common.Utilities.Pipelines;

internal class PipelinePartComparer<TMessage> : IComparer<PipelinePart<TMessage>>
{
    public int Compare(PipelinePart<TMessage>? x, PipelinePart<TMessage>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var a = x.Priority.CompareTo(y.Priority);
        return a == 0 ? x.Plug.GetHashCode().CompareTo(y.Plug.GetHashCode()) : a;
    }
}
