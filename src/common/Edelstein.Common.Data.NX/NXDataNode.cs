using System.Collections;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataNode : IDataNode
{
    private readonly INXNode _node;
    private readonly IDictionary<string, object> _hotfix;

    public string Name => _node.Name;
    public string File { get; }
    public string Path => _node.Parent == null ? File : $"{Parent.Path}/{_node.Name}";

    public IDataNode Parent => new NXDataNode(File, _node.Parent, _hotfix);
    public IEnumerable<IDataNode> Children => _node.Children.Select(c => new NXDataNode(File, c, _hotfix));

    public NXDataNode(string file, INXNode node, IDictionary<string, object> hotfix)
    {
        File = file;
        _node = node;
        _hotfix = hotfix;
    }

    public IEnumerator<IDataNode> GetEnumerator() =>
        Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IDataNode ResolveAll() => new NXDataNode(File, _node.ResolveAll(), _hotfix);

    public IDataNode? Resolve(string? path = null)
    {
        try
        {
            var subNode = _node.ResolvePath(path);
            return subNode == null ? null : new NXDataNode(File, subNode, _hotfix);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public T? Resolve<T>(string? path = null) where T : struct =>
        _hotfix.TryGetValue(path == null ? Path : $"{Path}/{path}", out var result)
            ? (T)result
            : _node.Resolve<T>(path);

    public T? ResolveOrDefault<T>(string? path = null) where T : class =>
        _hotfix.TryGetValue(path == null ? Path : $"{Path}/{path}", out var result)
            ? (T)result
            : _node.ResolveOrDefault<T>(path);
}
