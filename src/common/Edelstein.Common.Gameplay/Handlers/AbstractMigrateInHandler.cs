using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Pipelines;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handlers;

public class AbstractMigrateInHandler<TStage, TStageUser> : AbstractPluggedPacketHandler<TStageUser, UserOnMigrateIn<TStageUser>>
    where TStage : IStage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly ILogger _logger;
    private readonly IMigrationService _migrationService;
    private readonly ISessionService _sessionService;
    private readonly TStage _stage;

    public AbstractMigrateInHandler(
        IPipeline<UserOnMigrateIn<TStageUser>> pipeline, 
        ILogger<AbstractMigrateInHandler<TStage, TStageUser>> logger, 
        IMigrationService migrationService,
        ISessionService sessionService, 
        TStage stage
    ) : base(pipeline)
    {
        _logger = logger;
        _migrationService = migrationService;
        _sessionService = sessionService;
        _stage = stage;
    }

    public override short Operation => (short)PacketRecvOperations.MigrateIn;

    public override bool Check(TStageUser user) 
        => user is { IsMigrating: false, Account: null, AccountWorld: null, Character: null };

    protected override UserOnMigrateIn<TStageUser> Serialize(TStageUser user, IPacketReader reader)
        => new(
            user, 
            reader.ReadInt(),
            reader.Skip(18).ReadLong()
        );

    public override async Task Handle(IPipelineContext ctx, UserOnMigrateIn<TStageUser> message)
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
