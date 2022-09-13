using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Util.Pipelines;

public class Pipeline<TMessage> : IPipeline<TMessage>
{
    private readonly ICollection<PipelinePart<TMessage>> _parts;

    public Pipeline() => _parts = new SortedSet<PipelinePart<TMessage>>();

    public Pipeline(IPipelinePlug<TMessage> @default) : this() =>
        _parts.Add(new PipelinePart<TMessage>(PipelinePriority.Default, @default, true));

    public void Add(int priority, IPipelinePlug<TMessage> plug) =>
        _parts.Add(new PipelinePart<TMessage>(priority, plug));

    public void Add(IPipelinePlug<TMessage> plug) =>
        Add(PipelinePriority.Normal, plug);

    public void Remove(IPipelinePlug<TMessage> plug)
    {
        var part = _parts.FirstOrDefault(p => p.Plug == plug);
        if (part != null) _parts.Remove(part);
    }

    public async Task Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in _parts)
        {
            if (part.IsDefault && ctx.IsRequestedCancellationDefault) continue;
            await part.Plug.Handle(ctx, message);
            if (ctx.IsRequestedCancellation) break;
        }
    }
}
