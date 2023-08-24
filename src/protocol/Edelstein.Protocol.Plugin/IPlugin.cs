using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Plugin;

public interface IPlugin<in TContext> : IIdentifiable<string>
{
    Task OnInit(IPluginHost host, TContext ctx);
    Task OnStart(IPluginHost host, TContext ctx);
    Task OnStop();
}
