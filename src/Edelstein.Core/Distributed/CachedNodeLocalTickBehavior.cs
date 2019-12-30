using System;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeLocalTickBehavior<TState> : ITickBehavior
        where TState : INodeState
    {
        private readonly CachedNodeLocal<TState> _cachedNode;
        private readonly ICacheClient _cache;

        public CachedNodeLocalTickBehavior(CachedNodeLocal<TState> cachedNode, ICacheClient cache)
        {
            _cachedNode = cachedNode;
            _cache = cache;
        }

        public async Task<bool> TryTick()
        {
            await _cache.SetAddAsync<INodeState>(Scopes.NodeSet, _cachedNode.State, TimeSpan.FromMinutes(1));
            return true;
        }
    }
}