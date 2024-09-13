using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Plugin;

public interface IPlugin<TContext> : IRepositoryEntry<string>
{
    Task OnInit(IPluginHost<TContext> host, TContext ctx) => Task.CompletedTask;
    Task OnStart(IPluginHost<TContext> host, TContext ctx);
    Task OnStop();
}
