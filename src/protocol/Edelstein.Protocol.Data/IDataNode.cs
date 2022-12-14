namespace Edelstein.Protocol.Data;

public interface IDataNode : IEnumerable<IDataNode>, IDataProperty
{
    string Name { get; }
    string Path { get; }

    IDataNode Parent { get; }
    IEnumerable<IDataNode> Children { get; }

    IDataNode ResolveAll();
}
