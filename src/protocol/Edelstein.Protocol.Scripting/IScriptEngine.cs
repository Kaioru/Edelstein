using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptEngine
    {
        Task<object> Evaluate(string source, IScriptScope scope = null);
        Task<object> EvaluateFromFile(string path, IScriptScope scope = null);

        Task<IScript> Create(string source);
        Task<IScript> CreateFromFile(string path);
    }
}
