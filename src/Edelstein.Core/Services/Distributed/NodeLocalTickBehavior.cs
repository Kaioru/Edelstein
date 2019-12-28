using System;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Caching;

namespace Edelstein.Core.Services.Distributed
{
    public class NodeLocalTickBehavior<TState> : ITickBehavior
        where TState : INodeState
    {
        private readonly NodeLocal<TState> _node;
        private readonly ICacheClient _cache;

        public NodeLocalTickBehavior(NodeLocal<TState> node, ICacheClient cache)
        {
            _node = node;
            _cache = cache;
        }

        public async Task<bool> TryTick()
        {
            await _cache.SetAddAsync<INodeState>(Scopes.NodeSetKey, _node.State, TimeSpan.FromSeconds(15));
            return true;
        }
    }
}