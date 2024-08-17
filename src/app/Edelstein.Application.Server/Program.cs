using Edelstein.Application.Server;
using Edelstein.Application.Server.Commands;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

var builder = ProgramHostBuilder.CreateBuilder();
var registrar = new DependencyInjectionRegistrar(builder.Services);
var app = new CommandApp<StartCommand>(registrar);

app.Configure(c =>
{
    c
        .AddCommand<StartCommand>("start")
        .WithDescription("Starts the service daemon");
});

return await app.RunAsync(args);
