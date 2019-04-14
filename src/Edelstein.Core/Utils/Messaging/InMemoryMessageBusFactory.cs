using Foundatio.Messaging;

namespace Edelstein.Core.Utils.Messaging
{
    public class InMemoryMessageBusFactory : AbstractMessageBusFactory
    {
        protected override IMessageBus Create(string topic)
        {
            return new InMemoryMessageBus(new InMemoryMessageBusOptions
            {
                Topic = topic
            });
        }
    }
}