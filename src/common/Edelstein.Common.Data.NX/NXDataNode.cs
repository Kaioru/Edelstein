using System.Collections;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataNode : IDataNode
{
    private readonly INXNode _node;
    private readonly IDictionary<string, object> _hotfix;

    public string Name => _node.Name;
    public string Path => System.IO.Path.Join(_node.Parent.Name, _node.Name);

    public IDataNode Parent => new NXDataNode(_node.Parent, _hotfix);
    public IEnumerable<IDataNode> Children => _node.Children.Select(c => new NXDataNode(c, _hotfix));

    public NXDataNode(INXNode node, IDictionary<string, object> hotfix)
    {
        _node = node;
        _hotfix = hotfix;
    }

    public IEnumerator<IDataNode> GetEnumerator() =>
        Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IDataNode ResolveAll() => new NXDataNode(_node.ResolveAll(), _hotfix);

    public IDataNode? ResolvePath(string? path = null)
    {
        try
        {
            var subNode = _node.ResolvePath(path);
            return subNode == null ? null : new NXDataNode(subNode, _hotfix);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public T? Resolve<T>(string? path = null) where T : struct =>
        _hotfix.TryGetValue($"{Path}/{path}", out var result) ? (T)result : _node.Resolve<T>(path);

    public T? ResolveOrDefault<T>(string? path = null) where T : class =>
        _hotfix.TryGetValue($"{Path}/{path}", out var result) ? (T)result : _node.ResolveOrDefault<T>(path);
}
