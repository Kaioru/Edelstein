using System.Reflection;
using Autofac;
using Edelstein.Application.Server.Bootstraps;
using Edelstein.Application.Server.Configs;
using Edelstein.Common.Database;
using Edelstein.Common.Gameplay;
using Edelstein.Common.Gameplay.Game;
using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Gameplay.Game.Continents;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Quests;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop;
using Edelstein.Common.Gameplay.Shop.Commodities;
using Edelstein.Common.Gameplay.Trade;
using Edelstein.Common.Network.DotNetty.Transports;
using Edelstein.Common.Plugin;
using Edelstein.Common.Services.Auth;
using Edelstein.Common.Services.Server;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Contexts;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Gameplay.Trade.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server;

public class ProgramHost : IHostedService
{
    private readonly ICollection<IBootstrap> _bootstraps;
    private readonly ProgramConfig _config;
    private readonly ILifetimeScope _scope;

    public ProgramHost(ProgramConfig config, ILifetimeScope scope)
    {
        _bootstraps = new HashSet<IBootstrap>();
        _config = config;
        _scope = scope;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var programScope = _scope.BeginLifetimeScope(b =>
        {
            var assemblies = new List<Assembly> { Assembly.GetAssembly(typeof(AbstractStage<>))! };

            if (_config.LoginStages.Count > 0)
                assemblies.Add(Assembly.GetAssembly(typeof(LoginStage))!);
            if (_config.GameStages.Count > 0)
                assemblies.Add(Assembly.GetAssembly(typeof(GameStage))!);
            if (_config.ShopStages.Count > 0)
                assemblies.Add(Assembly.GetAssembly(typeof(ShopStage))!);
            if (_config.TradeStages.Count > 0)
                assemblies.Add(Assembly.GetAssembly(typeof(TradeStage))!);

            b
                .RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(ITemplateLoader)))
                .AsImplementedInterfaces()
                .SingleInstance();
        });
        var stages = new List<ProgramConfigStage>();

        stages.AddRange(_config.LoginStages);
        stages.AddRange(_config.GameStages);
        stages.AddRange(_config.ShopStages);
        stages.AddRange(_config.TradeStages);

        foreach (var stage in stages)
        {
            await using var stageScope = programScope.BeginLifetimeScope(b =>
            {
                b.RegisterGeneric(typeof(PluginManager<>)).As(typeof(IPluginManager<>)).SingleInstance();
                b.RegisterGeneric(typeof(PacketHandlerManager<>)).As(typeof(IPacketHandlerManager<>)).SingleInstance();
                b.RegisterGeneric(typeof(Pipeline<>)).As(typeof(IPipeline<>)).SingleInstance();

                b
                    .Register(c => new NettyTransportAcceptor(
                        c.Resolve<IAdapterInitializer>(),
                        new TransportVersion(stage.Version, stage.Patch, stage.Locale))
                    )
                    .As<ITransportAcceptor>()
                    .SingleInstance();

                b.RegisterType<StartStageBootstrap>().As<IBootstrap>().SingleInstance();
                b.Register(c => new StartServerBootstrap(
                    c.Resolve<ILogger<StartServerBootstrap>>(),
                    c.Resolve<ITickerManager>(),
                    c.Resolve<ITransportAcceptor>(),
                    stage
                )).As<IBootstrap>().SingleInstance();
                
                
                b.RegisterType<InventoryManager>().As<IInventoryManager>().SingleInstance();

                switch (stage)
                {
                    case ILoginStageOptions options:
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LoginStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LoginStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();

                        b
                            .RegisterInstance(options)
                            .As<ILoginStageOptions>()
                            .As<ProgramConfigStageLogin>()
                            .SingleInstance();
                        b.RegisterType<LoginContext>().SingleInstance();
                        b.RegisterType<LoginContextManagers>().SingleInstance();
                        b.RegisterType<LoginContextServices>().SingleInstance();
                        b.RegisterType<LoginContextRepositories>().SingleInstance();
                        b.RegisterType<LoginContextTemplates>().SingleInstance();
                        b.RegisterType<LoginContextPipelines>().SingleInstance();

                        b.RegisterType<LoginStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.RegisterInstance(new LoginStage(stage.ID))
                            .As<IStage<ILoginStageUser>>()
                            .As<ILoginStage>()
                            .SingleInstance();
                        
                        b.RegisterType<InitPluginBootstrap<LoginContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        b.RegisterType<StartPluginBootstrap<LoginContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageLogin>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                    case IGameStageOptions options:
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();

                        b.RegisterType<FieldManager>().As<IFieldManager>().SingleInstance();
                        b.RegisterType<ContiMoveManager>().As<IContiMoveManager>().SingleInstance();
                        b.RegisterType<ScriptedConversationManager>().As<INamedConversationManager>().SingleInstance();
                        b.RegisterType<SkillManager>().As<ISkillManager>().SingleInstance();
                        b.RegisterType<ModifiedQuestTimeManager>().As<IModifiedQuestTimeManager>().SingleInstance();
                        b.RegisterType<QuestManager>().As<IQuestManager>().SingleInstance();
                        b.RegisterType<MobQuestCacheManager>().As<IMobQuestCacheManager>().SingleInstance();
                        
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(GameStage))!)
                            .Where(t => t.IsClass && t.IsAssignableTo<ISkillHandler>())
                            .As<ISkillHandler>()
                            .SingleInstance();
                        
                        b
                            .RegisterInstance(options)
                            .As<IGameStageOptions>()
                            .As<ProgramConfigStageGame>()
                            .SingleInstance();
                        b.RegisterType<GameContext>().SingleInstance();
                        b.RegisterType<GameContextManagers>().SingleInstance();
                        b.RegisterType<GameContextServices>().SingleInstance();
                        b.RegisterType<GameContextRepositories>().SingleInstance();
                        b.RegisterType<GameContextTemplates>().SingleInstance();
                        b.RegisterType<GameContextPipelines>().SingleInstance();

                        b.RegisterType<GameStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.Register(c => new GameStage(stage.ID, c.Resolve<IFieldManager>()))
                            .As<IStage<IGameStageUser>>()
                            .As<IGameStage>()
                            .SingleInstance();
                        
                        b.RegisterType<InitPluginBootstrap<GameContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        b.RegisterType<StartPluginBootstrap<GameContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageGame>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                    case IShopStageOptions options:
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ShopStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ShopStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();
                        
                        b.RegisterType<NotSaleManager>().As<INotSaleManager>().SingleInstance();
                        b.RegisterType<CommodityManager>().As<ICommodityManager>().SingleInstance();
                        b.RegisterType<ModifiedCommodityManager>().As<IModifiedCommodityManager>().SingleInstance();
                        b.RegisterType<CashPackageManager>().As<ICashPackageManager>().SingleInstance();
                        
                        b
                            .RegisterInstance(options)
                            .As<IShopStageOptions>()
                            .As<ProgramConfigStageShop>()
                            .SingleInstance();
                        b.RegisterType<ShopContext>().SingleInstance();
                        b.RegisterType<ShopContextManagers>().SingleInstance();
                        b.RegisterType<ShopContextServices>().SingleInstance();
                        b.RegisterType<ShopContextRepositories>().SingleInstance();
                        b.RegisterType<ShopContextTemplates>().SingleInstance();
                        b.RegisterType<ShopContextPipelines>().SingleInstance();

                        b.RegisterType<ShopStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.Register(c => new ShopStage(stage.ID))
                            .As<IStage<IShopStageUser>>()
                            .As<IShopStage>()
                            .SingleInstance();
                        
                        b.RegisterType<InitPluginBootstrap<ShopContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        b.RegisterType<StartPluginBootstrap<ShopContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageShop>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                    case ITradeStageOptions options:
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TradeStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPacketHandler<>))
                            .SingleInstance();
                        b
                            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TradeStage))!)
                            .Where(t => t.IsClass)
                            .AsClosedTypesOf(typeof(IPipelinePlug<>))
                            .SingleInstance();
                        
                        b
                            .RegisterInstance(options)
                            .As<ITradeStageOptions>()
                            .As<ProgramConfigStageTrade>()
                            .SingleInstance();
                        b.RegisterType<TradeContext>().SingleInstance();
                        b.RegisterType<TradeContextManagers>().SingleInstance();
                        b.RegisterType<TradeContextServices>().SingleInstance();
                        b.RegisterType<TradeContextRepositories>().SingleInstance();
                        b.RegisterType<TradeContextTemplates>().SingleInstance();
                        b.RegisterType<TradeContextPipelines>().SingleInstance();

                        b.RegisterType<TradeStageUserInitializer>().As<IAdapterInitializer>().SingleInstance();
                        b.Register(c => new TradeStage(stage.ID))
                            .As<IStage<ITradeStageUser>>()
                            .As<ITradeStage>()
                            .SingleInstance();
                        
                        b.RegisterType<InitPluginBootstrap<TradeContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        b.RegisterType<StartPluginBootstrap<TradeContext>>()
                            .As<IBootstrap>()
                            .SingleInstance();

                        b.RegisterType<StartServerUpdateBootstrap<ProgramConfigStageTrade>>()
                            .As<IBootstrap>()
                            .SingleInstance();
                        break;
                }
            });

            foreach (var bootstrap in stageScope.Resolve<IEnumerable<IBootstrap>>())
                _bootstraps.Add(bootstrap);
        }

        _bootstraps.Add(new InitDatabaseBootstrap(
            programScope.Resolve<ILogger<InitDatabaseBootstrap>>(),
            programScope.Resolve<IDbContextFactory<GameplayDbContext>>(),
            programScope.Resolve<IDbContextFactory<AuthDbContext>>(),
            programScope.Resolve<IDbContextFactory<ServerDbContext>>(),
            _config
        ));
        _bootstraps.Add(new InitTickerBootstrap(programScope.Resolve<ITickerManager>()));
        _bootstraps.Add(new LoadTemplateBootstrap(
            programScope.Resolve<ILogger<LoadTemplateBootstrap>>(),
            programScope.Resolve<IEnumerable<ITemplateLoader>>())
        );

        foreach (var group in _bootstraps
                     .GroupBy(b => b.Priority)
                     .OrderBy(g => g.Key))
            await Task.WhenAll(group.AsParallel().Select(b => b.Start()));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var group in _bootstraps
                     .GroupBy(b => b.Priority)
                     .OrderByDescending(g => g.Key))
            await Task.WhenAll(group.AsParallel().Select(b => b.Stop()));
    }
}
