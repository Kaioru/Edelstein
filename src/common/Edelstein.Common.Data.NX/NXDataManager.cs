using System.Collections.Immutable;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataManager : IDataManager
{
    private readonly IDictionary<string, INXNode> _nodes;

    public NXDataManager(string path) =>
        _nodes = Directory
            .GetFiles(path, "*.nx")
            .ToImmutableDictionary(
                k => Path.GetFileNameWithoutExtension(k)!,
                d => (INXNode)new NXFile(d).Root
            );

    public T? Resolve<T>(string? path = null) where T : struct
    {
        var node = Resolve(path);
        if (node == null) return null;
        return node.Resolve<T>();
    }

    public T? ResolveOrDefault<T>(string? path = null) where T : class
    {
        var node = Resolve(path);
        if (node == null) return null;
        return node.ResolveOrDefault<T>();
    }


    public IDataNode? Resolve(string? path = null)
    {
        if (string.IsNullOrEmpty(path)) return null;
        var split = path.Split('/');
        return !_nodes.ContainsKey(split[0])
            ? null
            : new NXDataNode(_nodes[split[0]].ResolvePath(string.Join("/", split.Skip(1).ToArray())));
    }
}
