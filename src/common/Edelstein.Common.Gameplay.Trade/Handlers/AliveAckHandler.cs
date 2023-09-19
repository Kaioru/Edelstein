using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Trade.Handlers;

public class AliveAckHandler : AbstractAliveAckHandler<ITradeStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<ITradeStageUser>> pipeline) : base(pipeline)
    {
    }
}
