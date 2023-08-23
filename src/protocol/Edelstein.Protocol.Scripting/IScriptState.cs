namespace Edelstein.Protocol.Scripting;

public interface IScriptState : IDisposable
{
    void Register(string key, object value);

    Task<T> Call<T>(string name, params object[] args);
    Task<T> Evaluate<T>(string source);
}
