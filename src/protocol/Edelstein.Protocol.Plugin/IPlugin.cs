using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Plugin;

public interface IPlugin<TContext> : IIdentifiable<string>
{
    Task OnInit(IPluginHost<TContext> host, TContext ctx);
    Task OnStart(IPluginHost<TContext> host, TContext ctx);
    Task OnStop();

    Task<object?> Call(string type, params object[] args)
        => Task.FromResult<object?>(default);
}
