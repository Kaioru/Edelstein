using System;
using Edelstein.Core.Utils.Messaging;

namespace Edelstein.Core.Distributed
{
    public class CachedNodeRemote : AbstractCachedNode, INodeRemote
    {
        public INodeState State { get; }
        public DateTime Expire { get; }

        public CachedNodeRemote(
            INodeState state,
            DateTime expire,
            IMessageBusFactory busFactory
        ) : base(busFactory, state.Name)
        {
            State = state;
            Expire = expire;
        }
    }
}