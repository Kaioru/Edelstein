namespace Edelstein.Protocol.Caching
{
    public interface ICacheFactory
    {
        ICache CreateCache();
        ICache CreateScopedCache(string scope);
    }
}
