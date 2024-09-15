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

    c
        .AddBranch("database", c2 =>
        {
            c2
                .AddCommand<DatabaseMigrateCommand>("migrate")
                .WithDescription("Migrates the current database");
            c2
                .AddCommand<DatabaseDeleteCommand>("delete")
                .WithDescription("Deletes the current database");
        })
        .WithAlias("db");

    c.AddBranch("accounts", c2 =>
    {
        c2
            .AddCommand<AccountCreateCommand>("create")
            .WithDescription("Creates an account");
    });
});

return await app.RunAsync(args);
