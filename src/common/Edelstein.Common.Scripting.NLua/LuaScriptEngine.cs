using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Scripting.NLua;

public class LuaScriptEngine : IScriptEngine
{
    public Task<object> Evaluate(string source, IDictionary<string, object>? globals = null)
        => new LuaScript(source).Evaluate(globals);

    public Task<IScript> Create(string source = "")
        => Task.FromResult<IScript>(new LuaScript(source));
}
