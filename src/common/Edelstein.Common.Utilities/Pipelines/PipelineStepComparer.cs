using System.Collections.Generic;

namespace Edelstein.Common.Utilities.Pipelines;

internal class PipelineStepComparer<TMessage> : IComparer<PipelineStep<TMessage>>
{
    public int Compare(PipelineStep<TMessage>? x, PipelineStep<TMessage>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var a = x.Priority.CompareTo(y.Priority);
        return a == 0 ? x.Plug.GetHashCode().CompareTo(y.Plug.GetHashCode()) : a;
    }
}
