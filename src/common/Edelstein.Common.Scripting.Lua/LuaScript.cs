using Edelstein.Protocol.Scripting;
using MoonSharp.Interpreter;

namespace Edelstein.Common.Scripting.Lua;

public class LuaScript : IScript
{
    private readonly string _source;

    public LuaScript(string source) => _source = source;

    public Task<T> Evaluate<T>(IDictionary<string, object>? globals = null)
    {
        var script = new Script();
        if (globals != null)
            foreach (var global in globals)
                script.Globals[global.Key] = global.Value;
        return Task.FromResult(script.DoString(_source).ToObject<T>());
    }

    public Task<IScriptState> Run(IDictionary<string, object>? globals = null)
    {
        var script = new Script();
        if (globals != null)
            foreach (var global in globals)
                script.Globals[global.Key] = global.Value;
        script.DoString(_source);
        return Task.FromResult<IScriptState>(new LuaScriptState(script));
    }
}
