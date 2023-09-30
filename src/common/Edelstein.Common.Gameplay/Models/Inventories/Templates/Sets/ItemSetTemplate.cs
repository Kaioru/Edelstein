using System.Collections.Frozen;
using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Sets;

public record ItemSetTemplate : IItemSetTemplate
{
    public ItemSetTemplate(int id, IDataProperty property)
    {
        ID = id;

        SetCompleteCount = property.Resolve<int>("completeCount") ?? 0;

        Items = property.Resolve("ItemID")?.Children
            .Select(c => c.Resolve<int>() ?? 0)
            .ToFrozenSet() ?? FrozenSet<int>.Empty;
        Effects = property.Resolve("Effect")?.Children
            .ToFrozenDictionary(
                c => Convert.ToInt32(c.Name),
                c => (IItemSetTemplateEffect)new ItemSetTemplateEffect(Convert.ToInt32(c.Name), c.ResolveAll())
            ) ?? FrozenDictionary<int, IItemSetTemplateEffect>.Empty;
    }
    
    public int ID { get; }
    public int SetCompleteCount { get; }
    
    public ICollection<int> Items { get; }
    public IDictionary<int, IItemSetTemplateEffect> Effects { get; }
}
