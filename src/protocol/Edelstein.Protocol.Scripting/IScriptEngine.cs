namespace Edelstein.Protocol.Scripting;

public interface IScriptEngine
{
    string Extension { get; }

    Task<object> Evaluate(string source, IDictionary<string, object>? globals = null);
    Task<object?> EvaluateByName(string name, IDictionary<string, object>? globals = null);

    Task<IScript> Create(string source = "");
    Task<IScript?> CreateByName(string name);
}
