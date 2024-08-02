using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class Pipeline<TMessage> : IPipeline<TMessage>
{
    private readonly ICollection<PipeStep<TMessage>> _steps 
        = new SortedSet<PipeStep<TMessage>>(new PipeStepComparator<TMessage>());
    
    public Pipeline(IEnumerable<IPipe<TMessage>> pipes)
    {
        foreach (var pipe in pipes)
            _steps.Add(new PipeStep<TMessage>(PipePriority.Default, pipe));
    }
    
    public void Add(int priority, IPipe<TMessage> pipe) 
        => _steps.Add(new PipeStep<TMessage>(priority, pipe));
    
    public void Add(IPipe<TMessage> pipe) 
        => Add(PipePriority.Normal, pipe);
    
    public void Remove(IPipe<TMessage> pipe) 
    {
        var part = _steps.FirstOrDefault(p => p.Pipe == pipe);
        if (part != null) _steps.Remove(part);
    }
    
    public async Task<IPipelineContext> Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in _steps)
        {
            if (ctx.IsRequestedSkipToDefault && part.Priority != PipePriority.Default)
                continue;
            
            await part.Pipe.Handle(ctx, message);
            
            if (ctx.IsRequestedCancellation)
                break;
        }

        return ctx;
    }
}
