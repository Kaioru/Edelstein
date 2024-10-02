using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Shop.Templates;

public record ShopTemplate : IShopTemplate
{
    public int ID { get; }
    
    public ICollection<IShopTemplateItem> Items { get; }

    public ShopTemplate(int id, IDataNode node)
    {
        ID = id;
        Items = node
            .Select(n => (IShopTemplateItem)new ShopTemplateItem(n))
            .ToFrozenSet();
    }
}
