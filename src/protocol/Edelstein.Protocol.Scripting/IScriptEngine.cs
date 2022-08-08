namespace Edelstein.Protocol.Scripting;

public interface IScriptEngine
{
    Task<object> Evaluate(string source, IDictionary<string, object>? globals = null);
    Task<IScript> Create(string source = "");
}
