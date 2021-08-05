using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Scripting.NLua
{
    public class LuaScriptEngine : IScriptEngine
    {
        public Task<object> Evaluate(string source, IDictionary<string, object> globals = null)
            => new LuaScript(source).Evaluate(globals);

        public Task<object> EvaluateFromFile(string path, IDictionary<string, object> globals = null)
            => new LuaScript(File.ReadAllText(path)).Evaluate(globals);

        public Task<IScript> Create(string source = "")
            => Task.FromResult<IScript>(new LuaScript(source));

        public Task<IScript> CreateFromFile(string path)
            => Task.FromResult<IScript>(new LuaScript(File.ReadAllText(path)));
    }
}
