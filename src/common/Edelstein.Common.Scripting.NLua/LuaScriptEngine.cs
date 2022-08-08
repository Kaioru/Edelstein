using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Scripting.NLua;

public class LuaScriptEngine : IScriptEngine
{
    private readonly string _directory;

    public LuaScriptEngine(string directory) => _directory = directory;

    public string Extension => "lua";

    public Task<object> Evaluate(string source, IDictionary<string, object>? globals = null)
        => new LuaScript(source).Evaluate(globals);

    public async Task<object?> EvaluateByName(string name, IDictionary<string, object>? globals = null)
    {
        var file = Path.Join(_directory, Path.ChangeExtension(name, Extension));
        if (!File.Exists(file)) return null;
        return await new LuaScript(await File.ReadAllTextAsync(file)).Evaluate(globals);
    }

    public Task<IScript> Create(string source = "")
        => Task.FromResult<IScript>(new LuaScript(source));

    public async Task<IScript?> CreateByName(string name)
    {
        var file = Path.Join(_directory, Path.ChangeExtension(name, Extension));
        return !File.Exists(file)
            ? null
            : new LuaScript(await File.ReadAllTextAsync(file));
    }
}
