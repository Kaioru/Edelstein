using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop.Commodities;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class ShopOnPacketCashItemBuyRequestPlug : IPipelinePlug<ShopOnPacketCashItemBuyRequest>
{
    private readonly ICommodityManager _commodityManager;
    private readonly ITemplateManager<IItemTemplate> _itemTemplates;
    
    public ShopOnPacketCashItemBuyRequestPlug(ICommodityManager commodityManager, ITemplateManager<IItemTemplate> itemTemplates)
    {
        _commodityManager = commodityManager;
        _itemTemplates = itemTemplates;
    }
    
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemBuyRequest message)
    {
        var commodity = await _commodityManager.Retrieve(message.CommoditySN);
        var template = await _itemTemplates.Retrieve(commodity?.ItemID ?? 0);

        if (commodity == null) return;
        if (template == null) return;
        if (commodity.OnSale == false) return;
        if (message.User.AccountWorld?.Locker == null) return;
        if (message.User.AccountWorld.Locker.Items.Count >= message.User.AccountWorld.Locker.SlotMax) return;

        var item = commodity.ToItemLockerSlot(template);

        item.AccountID = message.User.Account?.ID ?? 0;
        item.CharacterID = message.User.Character?.ID ?? 0;

        message.User.AccountWorld.Locker.Items.Add((short)message.User.AccountWorld.Locker.Items.Count, item);

        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);
    
        p.WriteByte((byte)ShopResultOperations.Buy_Done);
        p.WriteItemLockerData(item);
        await message.User.Dispatch(p.Build());
    }
}
