using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Edelstein.Application.Server.Bindings;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Edelstein.Application.Server.Commands;

public class StartCommand : AsyncCommand<StartCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [Description("The path to directory with stage config files")]
        [CommandArgument(0, "[Path]")]
        public required string Path { get; init; } = "stages";
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var builder = SystemHostBuilder.CreateBuilder();
        
        foreach (var file in new DirectoryInfo(settings.Path).EnumerateFiles("*.json"))
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
                        builder.Services.AddSingleton<IHostedService>(p =>
                        {
                            var option = config.Get<StageSystemConfigLogin>();
                            var system = new LoginStageSystem(
                                option!,
                                p.GetRequiredService<LoginContext>()
                            );

                            return new SystemHost<ILoginStageSystem, ILoginStageSystemUser>(
                                p.GetRequiredService<ILogger<SystemHost<ILoginStageSystem, ILoginStageSystemUser>>>(),
                                system,
                                option!,
                                version
                            );
                        });
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

        await host.RunAsync();
        return 0;
    }
}
