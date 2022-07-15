using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Daemon.Server;

public class ProgramHost : IHostedService
{
    private readonly ICollection<ITransportAcceptor> _acceptors;
    private readonly IServiceCollection _collection;
    private readonly ProgramConfig _config;
    private readonly ILogger<ProgramHost> _logger;

    public ProgramHost(
        IOptions<ProgramConfig> options,
        ILogger<ProgramHost> logger,
        IServiceCollection collection
    )
    {
        _config = options.Value;
        _logger = logger;
        _collection = collection;
        _acceptors = new List<ITransportAcceptor>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var stage in _config.Stages)
        {
            var collection = new ServiceCollection();

            foreach (var descriptor in _collection) collection.Add(descriptor);
            switch (stage.Type)
            {
                case ProgramStageType.Login:
                    collection.AddSingleton<ILoginContext, LoginContext>();
                    collection.AddSingleton<ILoginContextPipelines, LoginContextPipelines>();
                    collection.AddSingleton<ILoginStage, LoginStage>();

                    collection.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var provider = collection.BuildServiceProvider();
            var adapter = provider.GetRequiredService<IAdapterInitializer>();
            var acceptor = new NettyTransportAcceptor(adapter, 95, "1", 8);

            await acceptor.Accept(stage.Host, stage.Port);
            _acceptors.Add(acceptor);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.WhenAll(_acceptors.Select(a => a.Close()));
}
