using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Caching
{
    public interface ICache
    {
        Task<string> Get(string key);
        Task<T> Get<T>(string key);

        Task Set(string key, string value, TimeSpan duration);
        Task Set(string key, string value, DateTime date);
        Task Set<T>(string key, T value, TimeSpan duration);
        Task Set<T>(string key, T value, DateTime date);

        Task Refresh(string key, TimeSpan duration);
        Task Refresh(string key, DateTime date);
        Task Remove(string key);
    }
}
