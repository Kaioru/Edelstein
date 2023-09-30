using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCShopTemplate : INPCShop, ITemplate
{
    public int ID { get; }
    
    public ICollection<INPCShopItem> Items { get; }

    public NPCShopTemplate(int id, IDataNode property)
    {
        ID = id;
        Items = property.Children
            .Select(p => (INPCShopItem)new NPCShopTemplateItem(Convert.ToInt32(p.Name), p.ResolveAll()))
            .ToImmutableSortedSet(new NPCShopTemplateItemComparer()) ?? ImmutableSortedSet<INPCShopItem>.Empty;
    }
}
