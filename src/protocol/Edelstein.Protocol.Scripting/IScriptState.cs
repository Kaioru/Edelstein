using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptState : IDisposable
    {
        Task<object> Evaluate(string source);
        Task<object> Call(string name, params object[] args);
    }
}
