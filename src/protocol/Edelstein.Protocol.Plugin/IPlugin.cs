using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Plugin;

public interface IPlugin<TContext> : IIdentifiable<string>
{
    Task OnStart(IPluginHost<TContext> host, TContext ctx);
    Task OnStop();
}
