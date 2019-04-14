using Foundatio.Messaging;

namespace Edelstein.Core.Distributed.Utils.Messaging
{
    public class InMemoryMessageBusFactory : AbstractMessageBusFactory
    {
        public override IMessageBus Create(string topic)
        {
            return new InMemoryMessageBus(new InMemoryMessageBusOptions
            {
                Topic = topic
            });
        }
    }
}