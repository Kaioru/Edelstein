using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Util.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server;

public class ProgramHost : IHostedService
{
    private readonly IServiceCollection _collection;
    private readonly ILogger<ProgramHost> _logger;

    public ProgramHost(ILogger<ProgramHost> logger, IServiceCollection collection)
    {
        _logger = logger;
        _collection = collection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var collection = new ServiceCollection();

        foreach (var descriptor in _collection) collection.Add(descriptor);

        collection.Scan(s => s
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IPacketHandler<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IPipelineAction<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

        collection.AddSingleton(typeof(IPacketHandlerManager<>), typeof(PacketHandlerManager<>));
        collection.AddSingleton(typeof(IPipeline<>), typeof(Pipeline<>));

        collection.AddSingleton<ILoginContext, LoginContext>();
        collection.AddSingleton<ILoginContextPipelines, LoginContextPipelines>();
        collection.AddSingleton<ILoginStage, LoginStage>();

        collection.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();

        var provider = collection.BuildServiceProvider();
        var adapter = provider.GetRequiredService<IAdapterInitializer>();
        var acceptor = new NettyTransportAcceptor(adapter, 95, "1", 8);

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
