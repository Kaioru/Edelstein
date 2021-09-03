using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Templates;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Hosting;
using Edelstein.Common.Network.DotNetty;
using Edelstein.Common.Services;
using Edelstein.Common.Services.Social;
using Edelstein.Common.Util.Ticks;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Edelstein.App.Login
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
                .ConfigureLogging(logging =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .MinimumLevel.Debug()
                        .CreateLogger();

                    logging.ClearProviders();
                    logging.AddSerilog();
                })

                .ConfigureDataStore()
                .ConfigureCaching()
                .ConfigureMessaging()
                .ConfigureParser()

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

                    builder.AddSingleton<ITemplateRepository<WorldTemplate>, WorldTemplateRepository>();
                    builder.AddSingleton<ITemplateRepository<ItemTemplate>, ItemTemplateRepository>();

                    builder.AddSingleton<ISessionInitializer, LoginSessionInitializer>();
                    builder.AddNettyAcceptor(config.Version, config.Patch, config.Locale);

                    builder.AddSingleton<
                        IPacketProcessor<LoginStage, LoginStageUser>,
                        PacketProcessor<LoginStage, LoginStageUser>
                    >();

                    builder.AddSingleton<LoginStageConfig>(config);
                    builder.AddSingleton<LoginStage>();
                    builder.AddHostedService<ProgramHost>();
                })
                .RunConsoleAsync();
    }
}
