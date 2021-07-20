using System.Threading.Tasks;

namespace Edelstein.Protocol.Caching.Serializer
{
    public interface ICacheSerializer
    {
        Task<T> Deserialize<T>(string data);
        Task<string> Serialize<T>(T obj);
    }
}
