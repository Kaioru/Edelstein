namespace Edelstein.Protocol.Util.Events;

public interface IEventConsumer<in TMessage>
{
    Task Handle(TMessage message);
}
