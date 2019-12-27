using System;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Distributed
{
    public class NodeLocal<TState> : AbstractNode, INodeLocal<TState>, ITickBehavior
        where TState : INodeState
    {
        private readonly ITicker _ticker;
        private readonly ICacheClient _cache;
        public TState State { get; }

        public NodeLocal(TState state, ICacheClient cache, IMessageBus bus) : base(cache, bus)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(10), this);
            _cache = cache;
            State = state;
        }

        public void Start()
        {
            TryTick().Wait();
            _ticker.Start();
        }

        public void Stop()
        {
            _ticker.Stop();
            _cache.SetRemoveAsync(Scopes.NodeSetKey, State).Wait();
        }

        public async Task<bool> TryTick()
        {
            await _cache.SetAddAsync<INodeState>(Scopes.NodeSetKey, State, TimeSpan.FromSeconds(15));
            return true;
        }
    }
}