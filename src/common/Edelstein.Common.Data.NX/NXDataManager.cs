using Duey;
using Edelstein.Protocol.Data;

namespace Edelstein.Common.Data.NX;

public class NXDataManager : IDataManager
{
    private readonly IDictionary<string, INXNode> _nodes;
    private readonly IDictionary<string, object> _hotfix;

    public NXDataManager(string path)
    {
        _nodes = Directory
            .GetFiles(path, "*.nx")
            .ToDictionary(
                k => Path.GetFileNameWithoutExtension(k)!,
                d => (INXNode)new NXFile(d).Root
            );
        _hotfix = _nodes.TryGetValue("Data", out var data)
            ? data
                .ToDictionary(
                    c => c.Name,
                    c => c.Resolve()
                )
            : new Dictionary<string, object>();
    }

    public T? Resolve<T>(string? path = null) where T : struct
    {
        var node = ResolvePath(path);
        return node?.Resolve<T>();
    }

    public T? ResolveOrDefault<T>(string? path = null) where T : class
    {
        var node = ResolvePath(path);
        return node?.ResolveOrDefault<T>();
    }

    public IDataNode? ResolvePath(string? path = null)
    {
        if (string.IsNullOrEmpty(path)) return null;
        var split = path.Split('/');
        return _nodes.TryGetValue(split[0], out var value)
            ? new NXDataNode(value.ResolvePath(string.Join("/", split.Skip(1).ToArray())), _hotfix)
            : null;
    }
}
