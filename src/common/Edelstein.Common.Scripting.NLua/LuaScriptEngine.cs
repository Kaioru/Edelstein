using System.IO;
using System.Threading.Tasks;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Scripting.NLua
{
    public class LuaScriptEngine : IScriptEngine
    {
        public Task<object> Evaluate(string source, IScriptScope scope = null)
            => new LuaScript(source).Evaluate(scope);

        public Task<object> EvaluateFromFile(string path, IScriptScope scope = null)
            => new LuaScript(File.ReadAllText(path)).Evaluate(scope);

        public Task<IScript> Create(string source)
            => Task.FromResult<IScript>(new LuaScript(source));

        public Task<IScript> CreateFromFile(string path)
            => Task.FromResult<IScript>(new LuaScript(File.ReadAllText(path)));
    }
}
