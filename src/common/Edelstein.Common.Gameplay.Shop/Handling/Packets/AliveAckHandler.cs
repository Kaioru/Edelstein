using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Packets;

public class AliveAckHandler : AbstractAliveAckHandler<IShopStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<IShopStageUser>> pipeline) : base(pipeline)
    {
    }
}
