using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Packets;

public class UserTransferFieldRequestHandler : AbstractPipedPacketHandler<IShopStageUser, ShopOnPacketUserTransferFieldRequest>
{    
    public UserTransferFieldRequestHandler(IPipeline<ShopOnPacketUserTransferFieldRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserTransferFieldRequest;
    
    public override bool Check(IShopStageUser user) => true;

    public override ShopOnPacketUserTransferFieldRequest? Serialize(IShopStageUser user, IPacketReader reader)
        => new(user);
}
