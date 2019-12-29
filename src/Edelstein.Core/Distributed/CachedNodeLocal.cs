using System;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeLocal<TState> : AbstractCachedNode, INodeLocal<TState>
        where TState : INodeState
    {
        private readonly ITicker _ticker;

        public TState State { get; }

        public CachedNodeLocal(TState state, ICacheClient cache, IMessageBusFactory busFactory) : base(cache, busFactory, state.Name)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(10), new CachedNodeLocalTickBehavior<TState>(this, cache));
            State = state;
        }

        protected void Start()
        {
            _ticker.ForceTick().Wait();
            _ticker.Start();
        }

        protected void Stop()
        {
            _ticker.Stop();
        }
    }
}