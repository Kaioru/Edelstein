using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Game.Commands;
using Edelstein.Common.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Common.Gameplay.Users.Skills.Templates;
using Edelstein.Common.Hosting;
using Edelstein.Common.Network.DotNetty;
using Edelstein.Common.Services;
using Edelstein.Common.Services.Social;
using Edelstein.Common.Util.Ticks;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Edelstein.App.Game
{
    internal static class Program
    {
        private static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", true);
                    builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
                    builder.AddCommandLine(args);
                })

                .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))

                .ConfigureDataStore()
                .ConfigureCaching()
                .ConfigureMessaging()
                .ConfigureParser()
                .ConfigureScripting()

                .ConfigureServices((context, builder) =>
                {
                    var config = context.Configuration.GetSection("Host").Get<ProgramConfig>();

                    builder.AddSingleton<IServerRegistry, ServerRegistry>();
                    builder.AddSingleton<ISessionRegistry, SessionRegistry>();
                    builder.AddSingleton<IMigrationRegistry, MigrationRegistry>();
                    builder.AddSingleton<IDispatchService, DispatchService>();

                    builder.AddSingleton<IInviteService, InviteService>();
                    builder.AddSingleton<IGuildService, GuildService>();
                    builder.AddSingleton<IPartyService, PartyService>();

                    builder.AddSingleton<IAccountRepository, AccountRepository>();
                    builder.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
                    builder.AddSingleton<ICharacterRepository, CharacterRepository>();

                    builder.AddSingleton<ITickerManager, TickerManager>();

                    builder.AddSingleton<ITemplateRepository<ItemTemplate>, ItemTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<ItemStringTemplate>, ItemStringTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<ItemOptionTemplate>, ItemOptionTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<ItemSetTemplate>, ItemSetTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<CharacterSkillTemplate>, CharacterSkillTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<FieldTemplate>, FieldTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<FieldStringTemplate>, FieldStringTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<ContiMoveTemplate>, ContiMoveTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<NPCTemplate>, NPCTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<MobTemplate>, MobTemplateRepository>();

                    builder.AddSingleton<ISessionInitializer, GameSessionInitializer>();
                    builder.AddNettyAcceptor(config.Version, config.Patch, config.Locale);

                    builder.AddSingleton<
                        IPacketProcessor<GameStage, GameStageUser>,
                        PacketProcessor<GameStage, GameStageUser>
                    >();
                    builder.AddSingleton<ICommandProcessor, CommandProcessor>();

                    builder.AddSingleton<GameStageConfig>(config);
                    builder.AddSingleton<GameStage>();
                    builder.AddHostedService<ProgramHost>();
                })
                .RunConsoleAsync();
    }
}
