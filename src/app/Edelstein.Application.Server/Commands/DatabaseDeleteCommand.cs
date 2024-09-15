using System.Threading.Tasks;
using Edelstein.Common.Database;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Edelstein.Application.Server.Commands;

public class DatabaseDeleteCommand(
    IDbContextFactory<GameDbContext> factory
) : AsyncCommand<DatabaseDeleteCommand.Settings>
{
    public class Settings : CommandSettings;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await using var db = await factory.CreateDbContextAsync();

        await db.Database.EnsureDeletedAsync();
        AnsiConsole.WriteLine("Database deleted");
        return 0;
    }
}
