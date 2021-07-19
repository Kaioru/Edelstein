using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptState
    {
        void Register(string key, object value);
        void Deregister(string key);

        Task<object> Evaluate(string source);
        Task<object> Call(string name, params object[] args);
    }
}
