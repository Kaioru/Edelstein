using Foundatio.Messaging;

namespace Edelstein.Core.Utils.Messaging
{
    public interface IMessageBusFactory
    {
        IMessageBus Build(string topic);
    }
}