using System.Collections.Generic;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptScope
    {
        IDictionary<string, object> Globals { get; }

        void Register(string key, object value);
        void Deregister(string key);
    }
}
