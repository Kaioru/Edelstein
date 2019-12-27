using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Distributed
{
    public class NodeRemote : AbstractNode, INodeRemote
    {
        public INodeState State { get; }

        public NodeRemote(INodeState state, ICacheClient cache, IMessageBus bus) : base(cache, bus)
        => State = state;
    }
}