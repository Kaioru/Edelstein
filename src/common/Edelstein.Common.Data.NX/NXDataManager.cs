using System.Collections.Frozen;
using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataManager : IDataManager
{
    private readonly IDictionary<string, INXNode> _nodes;

    public NXDataManager(string path) =>
        _nodes = Directory
            .GetFiles(path, "*.nx")
            .ToFrozenDictionary(
                k => Path.GetFileNameWithoutExtension(k)!,
                d => (INXNode)new NXFile(d).Root
            );

    public T? Resolve<T>(string? path = null) where T : struct
    {
        var node = Resolve(path);
        return node?.Resolve<T>();
    }

    public T? ResolveOrDefault<T>(string? path = null) where T : class
    {
        var node = Resolve(path);
        return node?.ResolveOrDefault<T>();
    }

    public IDataNode? Resolve(string? path = null)
    {
        if (string.IsNullOrEmpty(path)) return null;
        var split = path.Split('/');
        return _nodes.TryGetValue(split[0], out var value)
            ? new NXDataNode(value.ResolvePath(string.Join("/", split.Skip(1).ToArray())))
            : null;
    }
}
