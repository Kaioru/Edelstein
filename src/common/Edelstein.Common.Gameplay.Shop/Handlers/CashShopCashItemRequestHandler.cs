using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Gameplay.Shop.Types;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Shop.Handlers;

public class CashShopCashItemRequestHandler : IPacketHandler<IShopStageUser>
{
    private ILogger<CashShopCashItemRequestHandler> _logger;
    
    public CashShopCashItemRequestHandler(ILogger<CashShopCashItemRequestHandler> logger) => _logger = logger;
    
    public short Operation => (short)PacketRecvOperations.CashShopCashItemRequest;

    public bool Check(IShopStageUser user) => true;
    
    public async Task Handle(IShopStageUser user, IPacketReader reader)
    {
        var type = (ShopRequestOperations)reader.ReadByte();

        switch (type)
        {
            case ShopRequestOperations.Buy:
                await user.Context.Pipelines.ShopOnPacketCashItemBuyRequest.Process(new ShopOnPacketCashItemBuyRequest(
                    user,
                    (ShopCashType)reader.Skip(1).ReadInt(),
                    reader.ReadInt()
                ));
                break;
            case ShopRequestOperations.MoveLtoS:
                await user.Context.Pipelines.ShopOnPacketCashItemMoveLToSRequest.Process(new ShopOnPacketCashItemMoveLToSRequest(
                    user,
                    reader.ReadLong()
                ));
                break;
            case ShopRequestOperations.MoveStoL:
                await user.Context.Pipelines.ShopOnPacketCashItemMoveSToLRequest.Process(new ShopOnPacketCashItemMoveSToLRequest(
                    user,
                    reader.ReadLong(),
                    (ItemInventoryType)reader.ReadByte()
                ));
                break;
            default:
                _logger.LogWarning("Unhandled cash shop cash item request type {Type}", type);
                break;
        }

        await user.DispatchUpdateCash();
    }
}
