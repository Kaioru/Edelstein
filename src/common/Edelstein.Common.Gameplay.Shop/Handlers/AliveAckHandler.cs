using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handlers;

public class AliveAckHandler : AbstractAliveAckHandler<IShopStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<IShopStageUser>?> pipeline) : base(pipeline)
    {
    }
}
