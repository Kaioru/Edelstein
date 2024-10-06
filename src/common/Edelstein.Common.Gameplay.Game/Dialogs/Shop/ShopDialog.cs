using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Shop;

public class ShopDialog : IShopDialog
{
    private readonly int _templateID;
    private readonly Dictionary<int, IShopTemplateItem> _items;
    
    public ShopDialog(IShopTemplate template)
    {
        var pos = 0;
        
        _templateID = template.ID;
        _items = template.Items
            .OrderBy(i => i.ItemID)
            .ToDictionary(
                _ => pos++,
                i => i
            );
    }
    
    public Task OnOpen(IFieldUser user)
        => user.Dispatch(new OpenShopDlg
        {
            NPCTemplateID = _templateID,
            Items = _items.Values
                .Select(i => new OpenShopDlgItem
                {
                    ItemID = i.ItemID,
                    Price = i.Price,
                    DiscountRate = i.DiscountRate,
                    TokenItemID = i.TokenItemID,
                    TokenPrice = i.TokenPrice,
                    ItemPeriod = i.ItemPeriod,
                    LevelLimited = i.LevelLimited,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    MaxPerSlot = i.MaxPerSlot
                })
                .ToList()
        });

    public Task OnClose(IFieldUser user) => Task.CompletedTask;

    public Task Buy(IFieldUser user, UserShopRequestInfoBuy info)
    {
        Console.WriteLine(info);
        return user.ModifyStats(exclRequest: true);
    }

    public Task Sell(IFieldUser user, UserShopRequestInfoSell info)
    {
        Console.WriteLine(info);
        return user.ModifyStats(exclRequest: true);
    }

    public Task Recharge(IFieldUser user, UserShopRequestInfoRecharge info)
    {
        Console.WriteLine(info);
        return user.ModifyStats(exclRequest: true);
    }

    public Task Close(IFieldUser user) => user.EndDialog();
}
