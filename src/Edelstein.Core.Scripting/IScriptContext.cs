using System.Collections.Generic;

namespace Edelstein.Core.Scripting
{
    public interface IScriptContext
    {
        IDictionary<string, object> All();

        T Get<T>(string key);
        T Get<T>();

        void Register<T>(string key, T obj);
        void Register<T>(T obj);

        void Deregister(string key);
        void Deregister<T>(T obj);
    }
}