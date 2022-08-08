namespace Edelstein.Protocol.Scripting;

public interface IScriptState : IDisposable
{
    void Register(string key, object value);

    Task<object> Call(string name, params object[] args);
    Task<object> Evaluate(string source);
}
