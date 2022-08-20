using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Contracts.Types;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public class AbstractSocketOnMigrateOutPlug<TStageUser, TOptions> : IPipelinePlug<ISocketOnMigrateOut<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
    where TOptions : IStageContextOptions
{
    private readonly ILogger _logger;
    private readonly IMigrationService _migrationService;
    private readonly TOptions _options;

    public AbstractSocketOnMigrateOutPlug(
        ILogger<AbstractSocketOnMigrateOutPlug<TStageUser, TOptions>> logger,
        IMigrationService migrationService,
        TOptions options
    )
    {
        _logger = logger;
        _migrationService = migrationService;
        _options = options;
    }

    public virtual async Task Handle(IPipelineContext ctx, ISocketOnMigrateOut<TStageUser> message)
    {
        if (message.User.Account == null || message.User.AccountWorld == null || message.User.Character == null)
        {
            await message.User.Disconnect();
            return;
        }

        var migration = new Migration
        {
            AccountID = message.User.Account.ID,
            CharacterID = message.User.Character.ID,
            FromServerID = _options.ID,
            ToServerID = message.ServerID,
            Key = message.User.Key,
            Account = message.User.Account,
            AccountWorld = message.User.AccountWorld,
            Character = message.User.Character
        };
        var response = await _migrationService.Start(new MigrationStartRequest(migration));

        if (response.Result != MigrationResult.Success)
        {
            await message.User.Disconnect();
            _logger.LogDebug(
                "Failed to migrate out for user {Name} due to {Reason}",
                message.User.Character.Name, response.Result
            );
            return;
        }

        message.User.IsMigrating = true;
        _logger.LogDebug(
            "Migrated out character {Name} from service {From} to service {To} ",
            message.User.Character.Name, _options.ID, message.ServerID
        );
    }
}
