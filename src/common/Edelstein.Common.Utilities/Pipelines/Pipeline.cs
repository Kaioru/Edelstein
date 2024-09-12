using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class Pipeline<TMessage> : Pipework<TMessage>, IPipeline<TMessage>
{
    protected Pipeline() {}
    
    public Pipeline(IEnumerable<IPipe<TMessage>> pipes)
    {
        foreach (var pipe in pipes)
            Steps.Add(new PipeStep<TMessage>(PipePriority.Default, pipe));
    }
    
    public async Task<IPipelineContext> Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in Steps)
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
