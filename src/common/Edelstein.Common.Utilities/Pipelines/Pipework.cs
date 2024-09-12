using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class Pipework<TMessage> : IPipework<TMessage>
{
    internal readonly ICollection<PipeStep<TMessage>> Steps = new SortedSet<PipeStep<TMessage>>(new PipeStepComparator<TMessage>());
    
    public void Add(int priority, IPipe<TMessage> pipe) 
        => Steps.Add(new PipeStep<TMessage>(priority, pipe));
    
    public void Add(IPipe<TMessage> pipe) 
        => Add(PipePriority.Normal, pipe);
    
    public void Remove(IPipe<TMessage> pipe) 
    {
        var part = Steps.FirstOrDefault(p => p.Pipe == pipe);
        if (part != null) Steps.Remove(part);
    }
}
