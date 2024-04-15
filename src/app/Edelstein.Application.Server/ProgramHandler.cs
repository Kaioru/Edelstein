using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Application.Server.Bindings;
using Edelstein.Common.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Edelstein.Application.Server;

internal static class ProgramHandler
{
    public async static Task ExecuteRoot(FileInfo file)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddEdelsteinCommonUtilities();
        builder.Services.AddEdelsteinCommonGameplay();
        builder.Services.AddEdelsteinCommonGameplayLogin();

        var configFileInfos = (file.Attributes & FileAttributes.Directory) != 0
            ? file.Directory?.GetFiles() ?? Array.Empty<FileInfo>()
            : new[]{ file };
    
        foreach (var configFile in configFileInfos.Where(f => f.Extension == ".json"))
        {
            var version = new TransportVersion(95, "1", 8);
            var config = new ConfigurationBuilder()
                .AddJsonFile(configFile.FullName, false, false)
                .Build();

            switch (config["Type"])
            {
                case "Login":
                    builder.Services.AddHostedService(p =>
                    {
                        var loginConfig = new StageConfigLogin();
                        var loginSystem = new LoginStageSystem(loginConfig);
                
                        config.Bind(loginConfig);
                        
                        return new ServiceHostStage<ILoginStageUser, ILoginStageSystem>(
                            p.GetRequiredService<ILogger<ServiceHostStage<ILoginStageUser, ILoginStageSystem>>>(),
                            version,
                            loginConfig, 
                            loginSystem
                        );
                    });
                    break;
                default:
                    continue;
            }
        }
    
        var host = builder.Build();

        await host.RunAsync();
    }
}
