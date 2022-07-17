namespace Edelstein.Protocol.Data;

public interface IDataProperty
{
    Task<IDataNode?> Resolve(string? path = null);
    Task<T?> Resolve<T>(string? path = null) where T : struct;
    Task<T?> ResolveOrDefault<T>(string? path = null) where T : class;
}
