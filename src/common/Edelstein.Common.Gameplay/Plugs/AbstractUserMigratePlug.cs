using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Plugs;

public abstract class AbstractUserMigratePlug<TStage, TStageUser> : IPipelinePlug<UserMigrate<TStageUser>>
    where TStage : IStage<TStageUser>
    where TStageUser : class, IStageUser<TStageUser>
{
    private readonly ILogger _logger;
    private readonly IMigrationService _migrationService;
    private readonly TStage _stage;

    public AbstractUserMigratePlug(
        ILogger<AbstractUserMigratePlug<TStage, TStageUser>> logger,
        TStage stage,
        IMigrationService migrationService
    )
    {
        _logger = logger;
        _stage = stage;
        _migrationService = migrationService;
    }

    public async Task Handle(IPipelineContext ctx, UserMigrate<TStageUser> message)
    {

        if (message.User.Account == null || message.User.AccountWorld == null || message.User.Character == null)
        {
            await message.User.Disconnect();
            return;
        }

        var migration = new Migration(
            message.User.Account.ID,
            message.User.Character.ID,
            _stage.ID,
            message.ServerID,
            message.User.Key,
            message.User.Account,
            message.User.AccountWorld,
            message.User.Character
        );
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
        if (message.Packet != null)
            await message.User.Dispatch(message.Packet);
        _logger.LogDebug(
            "Migrated out character {Name} from service {From} to service {To} ",
            message.User.Character.Name, _stage.ID, message.ServerID
        );
    }
}
