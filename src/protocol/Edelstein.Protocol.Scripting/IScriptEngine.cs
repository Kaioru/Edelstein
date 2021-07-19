using System.Threading.Tasks;

namespace Edelstein.Protocol.Scripting
{
    public interface IScriptEngine
    {
        Task<object> Evaluate(string source);
        Task<object> EvaluateFromFile(string path);

        Task<IScript> Create(string source);
        Task<IScript> CreateFromFile(string path);
    }
}
