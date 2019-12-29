using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeRemote : AbstractCachedNode, INodeRemote
    {
        public INodeState State { get; }

        public CachedNodeRemote(INodeState state, ICacheClient cache, IMessageBusFactory busFactory) : base(cache, busFactory,
            state.Name)
            => State = state;
    }
}