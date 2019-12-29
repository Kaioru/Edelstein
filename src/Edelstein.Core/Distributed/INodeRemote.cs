namespace Edelstein.Core.Distributed
{
    public interface INodeRemote : INode
    {
        INodeState State { get; }
    }
}