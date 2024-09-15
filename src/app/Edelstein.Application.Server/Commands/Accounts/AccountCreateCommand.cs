using System.ComponentModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Edelstein.Application.Server.Commands.Accounts;

public class AccountCreateCommand : AsyncCommand<AccountCreateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [Description("The account username")]
        [CommandArgument(0, "[Username]")]
        public required string Username { get; init; }
        
        [Description("The account password")]
        [CommandArgument(0, "[Password]")]
        public required string Password { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var builder = ProgramHostBuilder.CreateBuilder();
        var host = builder.Build();
        var service = host.Services.GetRequiredService<IAuthService>();
        var response = await service.Register(new AuthServiceRequest
        {
            Username = settings.Username,
            Password = settings.Password
        });
        
        AnsiConsole.WriteLine($"Registered account {settings.Username} with result {response.Result}");
        return 0;
    }
}
