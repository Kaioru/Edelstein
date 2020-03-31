using System.Collections.Generic;
using Foundatio.Messaging;

namespace Edelstein.Core.Utils.Messaging
{
    public abstract class AbstractMessageBusFactory : IMessageBusFactory
    {
        private readonly IDictionary<string, IMessageBus> _busses;

        protected AbstractMessageBusFactory()
        {
            _busses = new Dictionary<string, IMessageBus>();
        }

        public IMessageBus Build(string topic)
        {
            _busses.TryGetValue(topic, out var bus);

            if (bus == null)
            {
                bus = Create(topic);
                _busses[topic] = bus;
            }

            return bus;
        }

        protected abstract IMessageBus Create(string topic);
    }
}