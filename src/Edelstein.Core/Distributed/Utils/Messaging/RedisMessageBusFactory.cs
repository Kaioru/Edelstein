using Foundatio.Messaging;
using StackExchange.Redis;

namespace Edelstein.Core.Distributed.Utils.Messaging
{
    public class RedisMessageBusFactory : AbstractMessageBusFactory
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisMessageBusFactory(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public override IMessageBus Create(string topic)
        {
            return new RedisMessageBus(new RedisMessageBusOptions
            {
                Subscriber = _connection.GetSubscriber(),
                Topic = topic
            });
        }
    }
}