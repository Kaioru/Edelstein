using Edelstein.Application.Server;
using Edelstein.Application.Server.Commands;
using Edelstein.Application.Server.Commands.Accounts;
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
    
    c.AddBranch("accounts", c =>
    {
        c
            .AddCommand<AccountCreateCommand>("create")
            .WithDescription("Creates an account");
    });
});

return await app.RunAsync(args);
