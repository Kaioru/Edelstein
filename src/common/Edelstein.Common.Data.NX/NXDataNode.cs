using System.Collections;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataNode : IDataNode
{
    private readonly INXNode _node;

    public NXDataNode(INXNode node) => _node = node;

    public IEnumerator<IDataNode> GetEnumerator() =>
        Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IDataNode Parent => new NXDataNode(_node.Parent);
    public IEnumerable<IDataNode> Children => _node.Children.Select(c => new NXDataNode(c));

    public string Name => _node.Name;
    public string Path => System.IO.Path.Join(_node.Parent.Name, _node.Name);

    public IDataNode ResolveAll() => new NXDataNode(_node.ResolveAll());

    public IDataNode? Resolve(string? path = null)
    {
        try
        {
            return new NXDataNode(_node.ResolvePath(path));
        }
        catch (Exception)
        {
            return null;
        }
    }

    public T? Resolve<T>(string? path = null) where T : struct =>
        _node.Resolve<T>(path);

    public T? ResolveOrDefault<T>(string? path = null) where T : class =>
        _node.ResolveOrDefault<T>(path);
}
