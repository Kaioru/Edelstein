using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Packets;

public class MigrateInHandler : AbstractMigrateInHandler<IShopStageUser>
{
    public MigrateInHandler(IPipeline<UserOnPacketMigrateIn<IShopStageUser>> pipeline) : base(pipeline)
    {
    }
}
