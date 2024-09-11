using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class Pipework<TMessage> : IPipework<TMessage>
{
    private readonly ICollection<PipeStep<TMessage>> _steps 
        = new SortedSet<PipeStep<TMessage>>(new PipeStepComparator<TMessage>());
    
    public void Add(int priority, IPipe<TMessage> pipe) 
        => _steps.Add(new PipeStep<TMessage>(priority, pipe));
    
    public void Add(IPipe<TMessage> pipe) 
        => Add(PipePriority.Normal, pipe);
    
    public void Remove(IPipe<TMessage> pipe) 
    {
        var part = _steps.FirstOrDefault(p => p.Pipe == pipe);
        if (part != null) _steps.Remove(part);
    }
}
