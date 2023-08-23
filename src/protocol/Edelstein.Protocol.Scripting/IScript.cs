namespace Edelstein.Protocol.Scripting;

public interface IScript
{
    Task<T> Evaluate<T>(IDictionary<string, object>? globals = null);
    Task<IScriptState> Run(IDictionary<string, object>? globals = null);
}
