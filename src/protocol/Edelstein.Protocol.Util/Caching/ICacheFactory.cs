namespace Edelstein.Protocol.Util.Caching
{
    public interface ICacheFactory
    {
        ICache CreateCache();
        ICache CreateScopedCache(string scope);
    }
}
