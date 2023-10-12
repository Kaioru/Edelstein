using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Gameplay.Trade.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Trade.Handling.Packets;

public class UserTransferFieldRequestHandler : AbstractPipedPacketHandler<ITradeStageUser, TradeOnPacketUserTransferFieldRequest>
{    
    public UserTransferFieldRequestHandler(IPipeline<TradeOnPacketUserTransferFieldRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserTransferFieldRequest;
    
    public override bool Check(ITradeStageUser user) => true;

    public override TradeOnPacketUserTransferFieldRequest? Serialize(ITradeStageUser user, IPacketReader reader)
        => new(user);
}
