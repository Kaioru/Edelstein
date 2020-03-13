using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Messaging;

namespace Edelstein.Core.Distributed
{
    public abstract class AbstractCachedNode : INode
    {
        protected IDictionary<string, INodeRemote> Remotes { get; }
        protected IMessageBus Bus { get; }

        private readonly IMessageBusFactory _busFactory;

        protected AbstractCachedNode(IMessageBusFactory busFactory, string name)
        {
            Remotes = new ConcurrentDictionary<string, INodeRemote>();
            _busFactory = busFactory;
            Bus = busFactory.Build($"{DistributedScopes.NodeMessaging}:{name}");
        }

        public Task<IEnumerable<INodeRemote>> GetPeers()
            => Task.FromResult(Remotes.Values
                .Where(p => p.Expire > DateTime.UtcNow));

        public Task SendMessage<T>(T message) where T : class
            => Bus.PublishAsync<T>(message);

        public Task SendMessages<T>(IEnumerable<T> messages) where T : class
            => Task.WhenAll(messages.Select(m => Bus.PublishAsync(m)));

        public async Task BroadcastMessage<T>(T message) where T : class
            => await Task.WhenAll((await GetPeers()).Select(p => p.SendMessage(message)));

        public async Task BroadcastMessages<T>(IEnumerable<T> messages) where T : class
            => await Task.WhenAll((await GetPeers()).Select(p => p.SendMessages(messages)));
    }
}