using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Shop.Commodities;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class ShopOnPacketCashItemBuyRequestPlug : IPipelinePlug<ShopOnPacketCashItemBuyRequest>
{
    private readonly ICommodityManager _commodityManager;
    private readonly ICashPackageManager _cashPackageManager;
    private readonly ITemplateManager<IItemTemplate> _itemTemplates;
    
    public ShopOnPacketCashItemBuyRequestPlug(ICommodityManager commodityManager, ICashPackageManager cashPackageManager, ITemplateManager<IItemTemplate> itemTemplates)
    {
        _commodityManager = commodityManager;
        _cashPackageManager = cashPackageManager;
        _itemTemplates = itemTemplates;
    }

    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemBuyRequest message)
    {
        var commodity = await _commodityManager.Retrieve(message.CommoditySN);
        var template = await _itemTemplates.Retrieve(commodity?.ItemID ?? 0);

        if (commodity == null) return;
        if (template == null) return;
        if (commodity.OnSale == false) return;
        if (commodity.Meso > 0) return;
        if (await _cashPackageManager.Retrieve(message.CommoditySN) != null) return;
        if (message.User.AccountWorld?.Locker == null) return;
        if (message.User.AccountWorld.Locker.Items.Count >= message.User.AccountWorld.Locker.SlotMax) return;
        if (!message.User.CheckCash(message.Cash, commodity.Price)) return;

        var item = commodity.ToItemLockerSlot(template);

        item.AccountID = message.User.Account?.ID ?? 0;
        item.CharacterID = message.User.Character?.ID ?? 0;

        message.User.AccountWorld.Locker.Items.Add(item);
        message.User.IncCash(message.Cash, -commodity.Price);

        using var packet = new PacketWriter(PacketSendOperations.CashShopCashItemResult);
    
        packet.WriteByte((byte)ShopResultOperations.Buy_Done);
        packet.WriteItemLockerData(item);
        await message.User.Dispatch(packet.Build());
    }
}
