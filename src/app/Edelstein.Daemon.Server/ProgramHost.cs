using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Daemon.Server;

public class ProgramHost : IHostedService
{
    private readonly ICollection<ITransportAcceptor> _acceptors;
    private readonly ProgramConfig _config;
    private readonly ILogger<ProgramHost> _logger;

    private readonly ILoginContext _loginContext;

    public ProgramHost(
        IOptions<ProgramConfig> options,
        ILogger<ProgramHost> logger,
        ILoginContext loginContext
    )
    {
        _config = options.Value;
        _logger = logger;
        _loginContext = loginContext;
        _acceptors = new List<ITransportAcceptor>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var stage in _config.Stages.OrderBy(s => s.Type))
        {
            var collection = new ServiceCollection();

            switch (stage.Type)
            {
                case ProgramStageType.Login:
                    collection.AddSingleton(_loginContext);
                    collection.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();
                    collection.AddSingleton<ILoginStage, LoginStage>();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var provider = collection.BuildServiceProvider();
            var adapter = provider.GetRequiredService<IAdapterInitializer>();
            var acceptor = new NettyTransportAcceptor(adapter, 95, "1", 8);

            await acceptor.Accept(stage.Host, stage.Port);

            _logger.LogInformation(
                "{id} socket acceptor bound at {host}:{port}",
                stage.ID, stage.Host, stage.Port
            );
            _acceptors.Add(acceptor);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.WhenAll(_acceptors.Select(a => a.Close()));
}
