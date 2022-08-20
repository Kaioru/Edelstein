namespace Edelstein.Protocol.Util.Events;

public interface IEvent<TMessage>
{
    void Add(IEventConsumer<TMessage> consumer);
    void Remove(IEventConsumer<TMessage> consumer);

    Task Publish(TMessage message);
}
