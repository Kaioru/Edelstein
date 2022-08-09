using Edelstein.Protocol.Scripting;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Loaders;

namespace Edelstein.Common.Scripting.MoonSharp;

public class LuaScriptEngine : IScriptEngine
{
    private readonly string _directory;

    public LuaScriptEngine(string directory)
    {
        _directory = directory;
        UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader
        {
            ModulePaths = new[] { Path.Join(directory, "?"), Path.Join(directory, $"?.{Extension}") }
        };
    }

    public string Extension => "lua";

    public Task<T> Evaluate<T>(string source, IDictionary<string, object>? globals = null)
        => new LuaScript(source).Evaluate<T>(globals);

    public async Task<T?> EvaluateByName<T>(string name, IDictionary<string, object>? globals = null)
    {
        var file = Path.Join(_directory, Path.ChangeExtension(name, Extension));
        if (!File.Exists(file)) return default;
        return await new LuaScript(await File.ReadAllTextAsync(file)).Evaluate<T>(globals);
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
