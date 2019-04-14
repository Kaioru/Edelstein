using Foundatio.Messaging;

namespace Edelstein.Core.Distributed.Utils.Messaging
{
    public interface IMessageBusFactory
    {
        IMessageBus Build(string topic);
    }
}