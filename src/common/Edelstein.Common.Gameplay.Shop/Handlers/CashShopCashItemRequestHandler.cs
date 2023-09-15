using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Shop.Handlers;

public class CashShopCashItemRequestHandler : IPacketHandler<IShopStageUser>
{
    private ILogger<CashShopCashItemRequestHandler> _logger;
    
    public CashShopCashItemRequestHandler(ILogger<CashShopCashItemRequestHandler> logger) => _logger = logger;
    
    public short Operation => (short)PacketRecvOperations.CashShopCashItemRequest;

    public bool Check(IShopStageUser user) => true;
    
    public Task Handle(IShopStageUser user, IPacketReader reader)
    {
        var type = (ShopRequestOperations)reader.ReadByte();

        switch (type)
        {
            default:
                _logger.LogWarning("Unhandled cash shop cash item request type {Type}", type);
                break;
        }

        return user.DispatchUpdateCash();
    }
}
