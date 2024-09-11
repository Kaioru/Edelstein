namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipework<out TMessage>
{
    void Add(int priority, IPipe<TMessage> pipe);
    void Add(IPipe<TMessage> pipe);
    void Remove(IPipe<TMessage> pipe);
}
