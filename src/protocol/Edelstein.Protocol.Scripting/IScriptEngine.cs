namespace Edelstein.Protocol.Scripting;

public interface IScriptEngine
{
    string Extension { get; }

    Task<T> Evaluate<T>(string source, IDictionary<string, object>? globals = null);
    Task<T?> EvaluateByName<T>(string name, IDictionary<string, object>? globals = null);

    Task<IScript> Create(string source = "");
    Task<IScript?> CreateByName(string name);
}
