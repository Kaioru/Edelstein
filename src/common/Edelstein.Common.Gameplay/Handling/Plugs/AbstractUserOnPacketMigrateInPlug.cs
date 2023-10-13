using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handling.Plugs;

public abstract class AbstractUserOnPacketMigrateInPlug<TStage, TStageUser> : IPipelinePlug<UserOnPacketMigrateIn<TStageUser>>
    where TStage : IStage<TStageUser>
    where TStageUser : class, IStageUser<TStageUser>
{
    private readonly ILogger _logger;
    private readonly TStage _stage;
    private readonly IMigrationService _migrationService;
    private readonly ISessionService _sessionService;
    private readonly IFriendService _friendService;
    private readonly IPartyService _partyService;

    public AbstractUserOnPacketMigrateInPlug(
        ILogger<AbstractUserOnPacketMigrateInPlug<TStage, TStageUser>> logger,
        TStage stage,
        IMigrationService migrationService,
        ISessionService sessionService, 
        IFriendService friendService, 
        IPartyService partyService
    )
    {
        _logger = logger;
        _stage = stage;
        _migrationService = migrationService;
        _sessionService = sessionService;
        _friendService = friendService;
        _partyService = partyService;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketMigrateIn<TStageUser> message)
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
        SetValues(message.User, migrationResponse.Migration);

        message.User.Friends = (await _friendService.Load(new FriendLoadRequest(message.User.Character.ID))).Friends;
        message.User.Party = (await _partyService.Load(new PartyLoadRequest(message.User.Character.ID))).PartyMembership;

        _logger.LogDebug(
            "Migrated in character {Name} from service {From} to service {To} ",
            message.User.Character.Name,
            migrationResponse.Migration.FromServerID,
            migrationResponse.Migration.ToServerID
        );

        await _stage.Enter(message.User);
    }
    
    public virtual void SetValues(TStageUser user, IMigration migration) {}
}
