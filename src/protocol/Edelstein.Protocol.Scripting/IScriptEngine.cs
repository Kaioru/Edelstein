using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptEngine
    {
        string RootDirectory { get; }

        Task<object> Evaluate(string source, IDictionary<string, object> globals = null);
        Task<object> EvaluateFromFile(string path, IDictionary<string, object> globals = null);

        Task<IScript> Create(string source = "");
        Task<IScript> CreateFromFile(string path);
    }
}
