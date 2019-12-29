using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Distributed
{
    public abstract class AbstractCachedNode : INode
    {
        private readonly ICacheClient _cache;
        private readonly IMessageBusFactory _busFactory;
        private readonly IMessageBus _bus;

        protected AbstractCachedNode(ICacheClient cache, IMessageBusFactory busFactory, string name)
        {
            _cache = cache;
            _busFactory = busFactory;
            _bus = busFactory.Build($"{Scopes.NodeMessaging}:{name}");
        }

        public async Task<IEnumerable<INodeRemote>> GetPeers()
        {
            if (!await _cache.ExistsAsync(Scopes.NodeSet)) return new List<INodeRemote>();
            return (await _cache.GetSetAsync<INodeState>(Scopes.NodeSet)).Value
                .Select(s => new CachedNodeRemote(s, _cache, _busFactory))
                .ToImmutableList();
        }

        public Task SendMessage<T>(T message) where T : class
            => _bus.PublishAsync<T>(message);

        public Task SendMessages<T>(IEnumerable<T> messages) where T : class
            => Task.WhenAll(messages.Select(m => _bus.PublishAsync(m)));

        public async Task BroadcastMessage<T>(T message) where T : class
            => await Task.WhenAll((await GetPeers()).Select(p => p.SendMessage(message)));

        public async Task BroadcastMessages<T>(IEnumerable<T> messages) where T : class
            => await Task.WhenAll((await GetPeers()).Select(p => p.SendMessages(messages)));
    }
}