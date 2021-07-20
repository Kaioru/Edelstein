using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Caching.Serializer
{
    public interface ICacheSerializer
    {
        Task<T> Deserialize<T>(string data);
        Task<string> Serialize<T>(T obj);
    }
}
