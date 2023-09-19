using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop.Commodities;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class ShopOnPacketCashItemBuyPackageRequestPlug : IPipelinePlug<ShopOnPacketCashItemBuyPackageRequest>
{
    private readonly ICommodityManager _commodityManager;
    private readonly ICashPackageManager _cashPackageManager;
    private readonly ITemplateManager<IItemTemplate> _itemTemplates;
    
    public ShopOnPacketCashItemBuyPackageRequestPlug(ICommodityManager commodityManager, ICashPackageManager cashPackageManager, ITemplateManager<IItemTemplate> itemTemplates)
    {
        _commodityManager = commodityManager;
        _cashPackageManager = cashPackageManager;
        _itemTemplates = itemTemplates;
    }
    
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemBuyPackageRequest message)
    {
        var commodity = await _commodityManager.Retrieve(message.CommoditySN);
        if (commodity == null) return;
        var cashPackage = await _cashPackageManager.Retrieve(commodity.ItemID);
        if (cashPackage == null) return;
        if (commodity.OnSale == false) return;
        if (commodity.Meso > 0) return;
        if (message.User.AccountWorld?.Locker == null) return;
        if (message.User.AccountWorld.Locker.Items.Count >= message.User.AccountWorld.Locker.SlotMax - cashPackage.SN.Count + 1) 
            return;
        if (!message.User.CheckCash(message.Cash, commodity.Price)) return;

        var items = new List<IItemLockerSlot>();
        
        foreach (var sn in cashPackage.SN)
        {
            var snCommodity = await _commodityManager.Retrieve(sn);
            if (snCommodity == null) continue;
            var snTemplate = await _itemTemplates.Retrieve(snCommodity.ItemID);
            if (snTemplate == null) continue;
            var item = commodity.ToItemLockerSlot(snTemplate);
            
            item.AccountID = message.User.Account?.ID ?? 0;
            item.CharacterID = message.User.Character?.ID ?? 0;
            items.Add(item);
            message.User.AccountWorld.Locker.Items.Add(item);
        }
        
        message.User.IncCash(message.Cash, -commodity.Price);

        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);
    
        p.WriteByte((byte)ShopResultOperations.BuyPackage_Done);
        p.WriteByte((byte)items.Count);
        foreach (var item in items)
            p.WriteItemLockerData(item);
        p.WriteShort(0);
        await message.User.Dispatch(p.Build());
    }
}
