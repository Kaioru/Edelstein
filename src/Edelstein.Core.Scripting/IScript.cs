using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Scripting
{
    public interface IScript
    {
        IDictionary<string, object> All();

        T Get<T>(string key);
        T Get<T>();

        void Register<T>(string key, T obj);
        void Register<T>(T obj);

        void Deregister(string key);
        void Deregister<T>(T obj);
        
        Task<object> Run();
    }
}