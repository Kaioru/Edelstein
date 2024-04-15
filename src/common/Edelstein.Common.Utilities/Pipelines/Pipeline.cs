using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Pipelines;
using Injectio.Attributes;

namespace Edelstein.Common.Utilities.Pipelines;

[RegisterScoped(ImplementationType = typeof(Pipeline<>), ServiceType = typeof(IPipeline<>))]
public class Pipeline<TMessage>() : IPipeline<TMessage>
{
    private readonly ICollection<PipelineStep<TMessage>> _steps = new SortedSet<PipelineStep<TMessage>>(new PipelineStepComparer<TMessage>());

    public Pipeline(IEnumerable<IPipelinePlug<TMessage>> plugs) : this()
    {
        foreach (var plug in plugs)
            _steps.Add(new PipelineStep<TMessage>(PipelinePriority.Reserved, plug));
    }
    
    public void Add(int priority, IPipelinePlug<TMessage> plug) 
        => _steps.Add(new PipelineStep<TMessage>(priority, plug));

    public void Add(IPipelinePlug<TMessage> plug)
        => Add(PipelinePriority.Normal, plug);

    public void Remove(IPipelinePlug<TMessage> plug)
    {
        var part = _steps.FirstOrDefault(p => p.Plug == plug);
        if (part != null) _steps.Remove(part);
    }

    public async Task<IPipelineContext> Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in _steps)
        {
            await part.Plug.Handle(ctx, message);
            if (ctx.IsRequestedCancellation)
                break;
        }

        return ctx;
    }
}
