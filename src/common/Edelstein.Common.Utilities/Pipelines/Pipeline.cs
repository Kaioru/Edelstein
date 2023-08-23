using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class Pipeline<TMessage> : IPipeline<TMessage>
{
    private readonly ICollection<PipelinePart<TMessage>> _parts;

    public Pipeline() => _parts = new SortedSet<PipelinePart<TMessage>>();
    public Pipeline(IEnumerable<IPipelinePlug<TMessage>> plugs) : this()
    {
        foreach (var plug in plugs)
            _parts.Add(new PipelinePart<TMessage>(PipelinePriority.Reserved, true, plug));
    }

    public void Add(int priority, IPipelinePlug<TMessage> plug) =>
        _parts.Add(new PipelinePart<TMessage>(priority, false, plug));

    public void Add(IPipelinePlug<TMessage> plug) =>
        Add(PipelinePriority.Normal, plug);

    public void Remove(IPipelinePlug<TMessage> plug)
    {
        var part = _parts.FirstOrDefault(p => p.Plug == plug);
        if (part != null) _parts.Remove(part);
    }

    public async Task<IPipelineContext> Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in _parts)
        {
            if (ctx.IsRequestedDefaultAction && !part.IsDefaultAction)
                continue;
            await part.Plug.Handle(ctx, message);
            if (ctx.IsRequestedCancellation || ctx.IsRequestedDefaultAction)
                break;
        }

        return ctx;
    }
}
