using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Application.Server.Bindings;
using Edelstein.Application.Server.Extensions;
using Edelstein.Common.Gameplay.Game;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Common.Utilities.Bootstrap;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Edelstein.Application.Server.Commands;

public class StartCommand : AsyncCommand<StartCommand.Settings>
{
    public class Settings(
        IOptions<ProgramHostConfig> config
    ) : CommandSettings
    {
        [Description("The path to directory with stage config files")]
        [CommandArgument(0, "[Path]")]
        public required string Path { get; init; } = config.Value.StageDirectory;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var builder = ProgramHostBuilder.CreateBuilder();
        
        foreach (var file in new DirectoryInfo(settings.Path)
                     .EnumerateFiles("*.json")
                     .OrderBy(f => f.Name))
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(file.FullName)
                .Build();
            var version = new TransportVersion(95, "1", 8);
            
            try
            {
                switch (config["Type"])
                {
                    case "Login":
                        builder.Services.AddSystemHostService<
                            ILoginStageSystem,
                            ILoginStageSystemUser,
                            ILoginStageSystemOptions,
                            LoginStageSystem,
                            LoginStageSystemConfig,
                            LoginContext
                        >(version, config);
                        break;
                    case "Game":
                        builder.Services.AddSystemHostService<
                            IGameStageSystem,
                            IGameStageSystemUser,
                            IGameStageSystemOptions,
                            GameStageSystem,
                            GameStageSystemConfig,
                            GameContext
                        >(version, config);
                        break;
                    default:
                        continue;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        var host = builder.Build();
        var loaders = host.Services.GetServices<IBootLoader>();
        
        await Task.WhenAll(loaders.AsParallel().Select(l => l.Load()));
        await host.RunAsync();
        return 0;
    }
}
