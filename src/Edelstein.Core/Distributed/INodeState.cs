namespace Edelstein.Core.Distributed
{
    public interface INodeState
    {
        string Name { get; }
        string Scope { get; }
    }
}