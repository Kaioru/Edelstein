using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Shop.Handlers;

public class CashShopQueryCashRequestHandler : IPacketHandler<IShopStageUser>
{
    public short Operation => (short)PacketRecvOperations.CashShopQueryCashRequest;
    
    public bool Check(IShopStageUser user) => true;

    public Task Handle(IShopStageUser user, IPacketReader reader)
        => user.DispatchUpdateCash();
}
