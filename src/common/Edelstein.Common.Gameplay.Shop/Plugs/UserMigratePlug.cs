using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Services.Migration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class UserMigratePlug : AbstractUserMigratePlug<IShopStage, IShopStageUser>
{
    public UserMigratePlug(
        ILogger<AbstractUserMigratePlug<IShopStage, IShopStageUser>> logger,
        IShopStage stage,
        IMigrationService migrationService
    ) : base(logger, stage, migrationService)
    {
    }
}
