using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPlugin<in TContext>
{
    Task OnStart(ILogger logger, TContext ctx);
    Task OnStop();
}
