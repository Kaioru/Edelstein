using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Util.Pipelines;

public class Pipeline<TMessage> : IPipeline<TMessage>
{
    private readonly IPipelineAction<TMessage>? _default;
    private readonly ICollection<PipelinePart<TMessage>> _parts;

    public Pipeline() => _parts = new SortedSet<PipelinePart<TMessage>>();
    public Pipeline(IPipelineAction<TMessage> @default) : this() => _default = @default;

    public void Add(int priority, IPipelineAction<TMessage> action) =>
        _parts.Add(new PipelinePart<TMessage>(priority, action));

    public void Remove(IPipelineAction<TMessage> action)
    {
        var part = _parts.FirstOrDefault(p => p.Action == action);
        if (part != null) _parts.Remove(part);
    }

    public async Task Process(TMessage message)
    {
        var ctx = new PipelineContext();

        foreach (var part in _parts)
        {
            await part.Action.Handle(ctx, message);
            if (ctx.IsRequestedCancellation) break;
        }

        if (_default == null || ctx.IsRequestedCancellation) return;
        await _default.Handle(ctx, message);
    }
}
