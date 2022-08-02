using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public class AbstractSocketOnMigrateInPlug<TStageUser, TStage, TOptions> : IPipelinePlug<ISocketOnMigrateIn<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
    where TStage : IStage<TStageUser>
    where TOptions : IStageContextOptions
{
    private readonly ILogger _logger;
    private readonly IMigrationService _migrationService;
    private readonly TOptions _options;
    private readonly ISessionService _sessionService;
    private readonly TStage _stage;


    public AbstractSocketOnMigrateInPlug(
        ILogger<AbstractSocketOnMigrateInPlug<TStageUser, TStage, TOptions>> logger,
        IMigrationService migrationService,
        ISessionService sessionService,
        TStage stage,
        TOptions options
    )
    {
        _logger = logger;
        _migrationService = migrationService;
        _sessionService = sessionService;
        _stage = stage;
        _options = options;
    }

    public async Task Handle(IPipelineContext ctx, ISocketOnMigrateIn<TStageUser> message)
    {
        if (message.User.Account != null || message.User.AccountWorld != null || message.User.Character != null)
        {
            await message.User.Disconnect();
            return;
        }

        var migrationResponse = await _migrationService.Claim(new MigrationClaimRequest(
            message.CharacterID,
            _options.ID,
            message.Key
        ));

        if (migrationResponse.Result != MigrationResult.Success || migrationResponse.Migration == null)
        {
            await message.User.Disconnect();
            _logger.LogDebug(
                "Failed to migrate in for character {ID}",
                message.CharacterID
            );
            return;
        }

        var sessionResponse = await _sessionService.UpdateServer(new SessionUpdateServerRequest(
            migrationResponse.Migration.Account.ID,
            _options.ID
        ));

        if (sessionResponse.Result != SessionResult.Success)
        {
            await message.User.Disconnect();
            _logger.LogDebug(
                "Failed to update session for character {ID}",
                message.CharacterID
            );
            return;
        }

        message.User.Key = migrationResponse.Migration.Key;
        message.User.Account = migrationResponse.Migration.Account;
        message.User.AccountWorld = migrationResponse.Migration.AccountWorld;
        message.User.Character = migrationResponse.Migration.Character;

        _logger.LogDebug(
            "Migrated in character {Name} from service {From} to service {To} ",
            message.User.Character.Name,
            migrationResponse.Migration.FromServerID,
            migrationResponse.Migration.ToServerID
        );

        await _stage.Enter(message.User);
    }
}
