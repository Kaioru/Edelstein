using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Distributed
{
    public abstract class AbstractNode : INode
    {
        private readonly ICacheClient _cache;
        private readonly IMessageBus _bus;

        protected AbstractNode(ICacheClient cache, IMessageBus bus)
        {
            _cache = cache;
            _bus = bus;
        }

        public async Task<IEnumerable<INodeRemote>> GetPeers()
        {
            if (!await _cache.ExistsAsync(Scopes.NodeSet)) return new List<INodeRemote>();
            return (await _cache.GetSetAsync<INodeState>(Scopes.NodeSet)).Value
                .Select(s => new NodeRemote(s, _cache, _bus))
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