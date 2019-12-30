using System.Threading.Tasks;
using Edelstein.Core.Utils.Ticks;
using Foundatio.Messaging;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeLocalTickBehavior<TState> : ITickBehavior
        where TState : INodeState
    {
        private readonly CachedNodeLocal<TState> _cachedNode;
        private readonly IMessageBus _bus;

        public CachedNodeLocalTickBehavior(CachedNodeLocal<TState> cachedNode, IMessageBus bus)
        {
            _cachedNode = cachedNode;
            _bus = bus;
        }

        public async Task<bool> TryTick()
        {
            await _bus.PublishAsync(new CachedNodeHeartbeatMessage
            {
                State = _cachedNode.State
            });
            return true;
        }
    }
}