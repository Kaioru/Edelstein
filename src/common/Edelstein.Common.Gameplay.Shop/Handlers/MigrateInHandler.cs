using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handlers;

public class MigrateInHandler : AbstractMigrateInHandler<IShopStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<IShopStageUser>> pipeline) : base(pipeline)
    {
    }
}
