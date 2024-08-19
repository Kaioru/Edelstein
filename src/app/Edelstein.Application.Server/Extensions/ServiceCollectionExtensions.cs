using Edelstein.Application.Server.Bindings;
using Edelstein.Application.Server.Services;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Application.Server.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static void AddSystemHostService<
        TStageSystem,
        TStageSystemUser, 
        TServerInfo, 
        TStageSystemImpl,
        TServerInfoImpl,
        TContext
    >
    (
        this IServiceCollection collection, 
        TransportVersion version,
        IConfiguration config
    ) 
        where TStageSystem : class, IStageSystem<TStageSystem, TStageSystemUser>
        where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>
        where TServerInfo : class, IServerInfo
        where TStageSystemImpl : class, TStageSystem
        where TServerInfoImpl : class, TServerInfo
        where TContext : class
    {
        collection.AddHostedService(p =>
        {
            var subCollection = new ServiceCollection();

            subCollection.AddSingleton<TServerInfo>(config.Get<TServerInfoImpl>()!);
            subCollection.AddSingleton(p.GetRequiredService<TContext>());
            subCollection.AddSingleton<TStageSystem, TStageSystemImpl>();
            
            var subProvider = subCollection.BuildServiceProvider();

            return new SystemHostService<TStageSystem, TStageSystemUser, TContext>(
                p.GetRequiredService<ILogger<SystemHostService<TStageSystem, TStageSystemUser, TContext>>>(),
                p.GetRequiredService<IOptions<ProgramHostConfig>>(),
                subProvider.GetRequiredService<TStageSystem>(),
                subProvider.GetRequiredService<TServerInfo>(),
                version,
                p.GetRequiredService<IPluginManager<TContext>>(),
                subProvider.GetRequiredService<TContext>()
            );
        });
    }
}
