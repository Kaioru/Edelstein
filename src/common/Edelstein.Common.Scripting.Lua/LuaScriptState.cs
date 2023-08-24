using Edelstein.Protocol.Scripting;
using MoonSharp.Interpreter;

namespace Edelstein.Common.Scripting.Lua;

public class LuaScriptState : IScriptState
{
    private readonly Script _script;

    public LuaScriptState(Script script) => _script = script;

    public void Register(string key, object value) =>
        _script.Globals[key] = value;

    public Task<T> Call<T>(string name, params object[] args) =>
        Task.FromResult(_script.Call(name, args).ToObject<T>());

    public Task<T> Evaluate<T>(string source) =>
        Task.FromResult(_script.DoString(source).ToObject<T>());

    public void Dispose()
    {
    }
}
