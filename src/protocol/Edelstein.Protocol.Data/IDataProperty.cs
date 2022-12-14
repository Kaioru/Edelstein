namespace Edelstein.Protocol.Data;

public interface IDataProperty
{
    IDataNode? ResolvePath(string? path = null);

    T? Resolve<T>(string? path = null) where T : struct;
    T? ResolveOrDefault<T>(string? path = null) where T : class;
}
