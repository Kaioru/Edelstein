using System.Threading.Tasks;

namespace Edelstein.Daemon.Server.Bootstraps;

public class ServerVoidBootstrap : IBootstrap
{
    public Task Start() => Task.CompletedTask;
    public Task Stop() => Task.CompletedTask;
}
