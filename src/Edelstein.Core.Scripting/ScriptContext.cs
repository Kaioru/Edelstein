using System.Collections.Generic;
using System.Collections.Immutable;

namespace Edelstein.Core.Scripting
{
    public class ScriptContext : IScriptContext
    {
        private readonly IDictionary<string, object> _globals;

        public ScriptContext()
            => _globals = new Dictionary<string, object>();

        public IDictionary<string, object> All()
            => _globals.ToImmutableDictionary();

        public T Get<T>(string key)
            => (T) _globals[key];

        public T Get<T>()
            => (T) _globals[typeof(T).Name];

        public void Register<T>(string key, T obj)
            => _globals.Add(key, obj);

        public void Register<T>(T obj)
            => _globals.Add(typeof(T).Name, obj);

        public void Deregister(string key)
            => _globals.Remove(key);

        public void Deregister<T>(T obj)
            => _globals.Remove(typeof(T).Name);
    }
}