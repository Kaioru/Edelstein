using System.Diagnostics;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Database.Repositories;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Contexts;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Session;
using Edelstein.Common.Util.Pipelines;
using Edelstein.Common.Util.Templates;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
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
        var stages = new List<AbstractProgramConfigStage>();

        stages.AddRange(_config.LoginStages);

        foreach (var stage in stages)
        {
            var collection = new ServiceCollection { _collection };

            collection.Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo(typeof(IPacketHandler<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IPipelineAction<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                .AddClasses(c => c.AssignableTo<ITemplateLoader>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
            );

            collection.AddSingleton(typeof(IPacketHandlerManager<>), typeof(PacketHandlerManager<>));
            collection.AddSingleton(typeof(IPipeline<>), typeof(Pipeline<>));
            collection.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));

            collection.AddSingleton<IAccountRepository, AccountRepository>();

            collection.AddSingleton<IAuthService, AuthService>();
            collection.AddSingleton<ISessionService, SessionService>();

            switch (stage)
            {
                case ILoginContextOptions options:
                    collection.AddSingleton(options);
                    collection.AddSingleton<ILoginContext, LoginContext>();
                    collection.AddSingleton<ILoginContextPipelines, LoginContextPipelines>();
                    collection.AddSingleton<ILoginContextServices, LoginContextServices>();
                    collection.AddSingleton<ILoginContextTemplates, LoginContextTemplates>();
                    collection.AddSingleton<IAdapterInitializer, LoginStageUserInitializer>();
                    collection.AddSingleton<ILoginStage, LoginStage>();
                    break;
            }

            var provider = collection.BuildServiceProvider();
            var loaders = provider.GetServices<ITemplateLoader>();

            foreach (var loader in loaders)
            {
                var stopwatch = new Stopwatch();
                var count = await loader.Load();

                _logger.LogInformation(
                    "{Loader} initialized {Count} templates in {Elapsed}",
                    loader.GetType().Name, count, stopwatch.Elapsed
                );
            }

            var adapter = provider.GetRequiredService<IAdapterInitializer>();
            var acceptor = new NettyTransportAcceptor(adapter, stage.Version, stage.Patch, stage.Locale);

            await acceptor.Accept(stage.Host, stage.Port);

            _logger.LogInformation(
                "{ID} socket acceptor bound at {Host}:{Port}",
                stage.ID, stage.Host, stage.Port
            );
            _acceptors.Add(acceptor);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.WhenAll(_acceptors.Select(a => a.Close()));
}
