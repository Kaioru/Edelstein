using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Shop.Commodities;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public static class CommodityConverters
{
    public static IItemLockerSlot ToItemLockerSlot(this ICommodity commodity, IItemTemplate template)
    {
        var item = template.ToItemSlot();
        var slot = new ItemLockerSlot { Item = item };

        if (item is IItemSlotBase itemBase)
        {
            if (commodity.Period > 0)
                itemBase.DateExpire = DateTime.UtcNow.AddDays(commodity.Period);
            itemBase.CashItemSN = new Random().NextInt64();
        }

        slot.CommodityID = commodity.ID;

        return slot;
    }
}
