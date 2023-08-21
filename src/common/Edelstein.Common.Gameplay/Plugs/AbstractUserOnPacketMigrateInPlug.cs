using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Plugs;

public class AbstractUserOnPacketMigrateInPlug<TStage, TStageUser> : IPipelinePlug<UserOnPacketMigrateIn<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
    where TStage : IStage<TStageUser>
{
    private readonly ILogger _logger;
    private readonly TStage _stage;
    private readonly IMigrationService _migrationService;
    private readonly ISessionService _sessionService;
    
    public AbstractUserOnPacketMigrateInPlug(
        ILogger<AbstractUserOnPacketMigrateInPlug<TStage, TStageUser>> logger, 
        TStage stage, 
        IMigrationService migrationService,
        ISessionService sessionService
    )
    {
        _logger = logger;
        _stage = stage;
        _migrationService = migrationService;
        _sessionService = sessionService;
    }

    public virtual async Task Handle(IPipelineContext ctx, UserOnPacketMigrateIn<TStageUser> message)
    {
        if (message.User.Account != null || message.User.AccountWorld != null || message.User.Character != null)
        {
            await message.User.Disconnect();
            return;
        }

        var migrationResponse = await _migrationService.Claim(new MigrationClaimRequest(
            message.CharacterID,
            _stage.ID,
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

        var sessionServerResponse = await _sessionService.UpdateServer(new SessionUpdateServerRequest(
            migrationResponse.Migration.Account.ID,
            _stage.ID
        ));
        var sessionCharacterResponse = await _sessionService.UpdateCharacter(new SessionUpdateCharacterRequest(
            migrationResponse.Migration.Account.ID,
            message.CharacterID
        ));

        if (
            sessionServerResponse.Result != SessionResult.Success ||
            sessionCharacterResponse.Result != SessionResult.Success
        )
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
