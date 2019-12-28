using System;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Distributed
{
    public class NodeLocal<TState> : AbstractNode, INodeLocal<TState>
        where TState : INodeState
    {
        private readonly ITicker _ticker;

        public TState State { get; }

        public NodeLocal(TState state, ICacheClient cache, IMessageBus bus) : base(cache, bus)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(10), new NodeLocalTickBehavior<TState>(this, cache));
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