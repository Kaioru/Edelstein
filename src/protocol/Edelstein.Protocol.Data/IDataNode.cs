namespace Edelstein.Protocol.Data;

public interface IDataNode : IEnumerable<IDataNode>, IDataProperty
{
    IDataNode Parent { get; }

    string Name { get; }
    string Path { get; }

    IDataNode ResolveAll();
}
