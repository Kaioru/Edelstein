using Foundatio.Messaging;
using StackExchange.Redis;

namespace Edelstein.Core.Utils.Messaging
{
    public class RedisMessageBusFactory : AbstractMessageBusFactory
    {
        private readonly ISubscriber _connection;

        public RedisMessageBusFactory(ConnectionMultiplexer connection)
        {
            _connection = connection.GetSubscriber();
        }

        protected override IMessageBus Create(string topic)
        {
            return new RedisMessageBus(new RedisMessageBusOptions
            {
                Subscriber = _connection,
                Topic = topic
            });
        }
    }
}