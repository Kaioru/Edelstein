using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Util.Events;

public class Event<TMessage> : IEvent<TMessage>
{
    private readonly ICollection<IEventConsumer<TMessage>> _consumers;

    public Event() => _consumers = new List<IEventConsumer<TMessage>>();

    public void Add(IEventConsumer<TMessage> consumer) =>
        _consumers.Add(consumer);

    public void Remove(IEventConsumer<TMessage> consumer) =>
        _consumers.Remove(consumer);

    public Task Publish(TMessage message) =>
        _ = Task.WhenAll(_consumers
            .AsParallel()
            .Select(h => h.Handle(message)));
}
