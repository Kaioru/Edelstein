using System.Collections.Generic;

namespace Edelstein.Common.Utilities.Pipelines;

internal class PipeStepComparator<TMessage> : IComparer<PipeStep<TMessage>>
{
    public int Compare(PipeStep<TMessage>? x, PipeStep<TMessage>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Priority.CompareTo(y.Priority);
    }
}
