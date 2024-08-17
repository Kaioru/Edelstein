using System.Reflection;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Edelstein.Application.Server;

internal static class SystemHostBuilder
{
    internal static HostApplicationBuilder CreateBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        
        builder.Services.AddSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));
        builder.Services.AddSingleton(typeof(ITemplateManagerContext<>), typeof(TemplateManagerContext<>));
        builder.Services.AddSingleton(typeof(ITemplateManager<>), typeof(TemplateManager<>));

        builder.Services.AddScoped(typeof(IPacketHandlerManager<,>), typeof(PacketHandlerManager<,>));
        builder.Services.AddScoped(typeof(IPipeline<>), typeof(Pipeline<>));
        builder.Services.Scan(scan => scan
            .FromAssemblyDependencies(Assembly.GetEntryAssembly()!)
            .AddClasses(classes => classes.AssignableTo(typeof(IPacketHandlerManagerEntry<,>))).AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(IPipe<>))).AsImplementedInterfaces()
            .WithScopedLifetime());
        
        builder.Services.AddScoped<LoginContext>();
        builder.Services.AddScoped<LoginContextPipelines>();

        return builder;
    }
}
