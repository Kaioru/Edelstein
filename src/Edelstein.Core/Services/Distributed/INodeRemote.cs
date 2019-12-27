namespace Edelstein.Core.Services.Distributed
{
    public interface INodeRemote : INode
    {
        INodeState State { get; }
    }
}