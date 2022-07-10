using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScript
    {
        Task<object> Evaluate(IDictionary<string, object> globals = null);
        Task<IScriptState> Run(IDictionary<string, object> globals = null);
    }
}
