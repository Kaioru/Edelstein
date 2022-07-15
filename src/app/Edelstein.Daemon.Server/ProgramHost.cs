using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Network.DotNetty.Transports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server;

public class ProgramHost : IHostedService
{
    private readonly ILogger<ProgramHost> _logger;

    public ProgramHost(ILogger<ProgramHost> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var initializer = new LoginStageUserInitializer();
        var acceptor = new NettyTransportAcceptor(initializer, 95, "1", 8);

        acceptor.Accept("127.0.0.1", 8484).Wait(cancellationToken);

        _logger.LogInformation("Host has started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Host has stopped");
        return Task.CompletedTask;
    }
}
