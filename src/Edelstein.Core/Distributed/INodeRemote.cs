using System;

namespace Edelstein.Core.Distributed
{
    public interface INodeRemote : INode
    {
        INodeState State { get; set; }
        DateTime Expire { get; set; }
    }
}