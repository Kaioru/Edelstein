using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Trade.Handling.Packets;

public class AliveAckHandler : AbstractAliveAckHandler<ITradeStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<ITradeStageUser>> pipeline) : base(pipeline)
    {
    }
}
