using Edelstein.Application.Server.Commands;
using Spectre.Console.Cli;

var app = new CommandApp<StartCommand>();

app.Configure(c =>
{
    c
        .AddCommand<StartCommand>("start")
        .WithDescription("Starts the service daemon");
});

return await app.RunAsync(args);
