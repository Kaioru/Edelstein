namespace Edelstein.Protocol.Utilities.Events;

public interface IEventConsumer<in TMessage>
{
    Task Handle(TMessage message);
}
