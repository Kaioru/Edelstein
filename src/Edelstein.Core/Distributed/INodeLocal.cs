namespace Edelstein.Core.Distributed
{
    public interface INodeLocal<out TState> : INode
        where TState : INodeState
    {
        TState State { get; }
    }
}