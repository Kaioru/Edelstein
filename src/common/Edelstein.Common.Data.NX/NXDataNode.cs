using System.Collections;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataNode : IDataNode
{
    private readonly INXNode _node;

    public NXDataNode(INXNode node) => _node = node;

    public IEnumerator<IDataNode> GetEnumerator() =>
        _node.Children.Select(c => new NXDataNode(c)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IDataNode Parent => new NXDataNode(_node.Parent);

    public string Name => _node.Name;
    public string Path => System.IO.Path.Join(_node.Parent.Name, _node.Name);

    public IDataNode ResolveAll() => new NXDataNode(_node.ResolveAll());

    public Task<IDataNode?> Resolve(string? path = null)
    {
        try
        {
            return Task.FromResult<IDataNode?>(new NXDataNode(_node.ResolvePath(path)));
        }
        catch (Exception)
        {
            return Task.FromResult<IDataNode?>(null);
        }
    }

    public Task<T?> Resolve<T>(string? path = null) where T : struct =>
        Task.Run(() => _node.Resolve<T>(path));

    public Task<T?> ResolveOrDefault<T>(string? path = null) where T : class =>
        Task.Run<T?>(() => _node.ResolveOrDefault<T>(path));
}
