using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Session;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class UserOnPacketMigrateInPlug : AbstractUserOnPacketMigrateInPlug<IShopStage, IShopStageUser>
{
    public UserOnPacketMigrateInPlug(ILogger<AbstractUserOnPacketMigrateInPlug<IShopStage, IShopStageUser>> logger, IShopStage stage, IMigrationService migrationService, ISessionService sessionService) : base(logger, stage, migrationService, sessionService)
    {
    }

    public override void SetValues(IShopStageUser user, IMigration migration)
        => user.FromServerID = migration.FromServerID;
}
