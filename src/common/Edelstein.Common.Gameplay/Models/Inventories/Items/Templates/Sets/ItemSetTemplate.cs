using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates.Sets;

public record ItemSetTemplate : IItemSetTemplate
{
    public ItemSetTemplate(int id, IDataNode node)
    {
        ID = id;

        SetCompleteCount = node.ResolveInt("completeCount") ?? 0;

        Items = node.ResolvePath("ItemID")?.Children
            .Select(c => c.ResolveInt() ?? 0)
            .ToImmutableList() ?? ImmutableList<int>.Empty;
        Effects = node.ResolvePath("Effect")?.Children
            .ToImmutableDictionary(
                c => Convert.ToInt32(c.Name),
                c => (IItemSetTemplateEffect)new ItemSetTemplateEffect(Convert.ToInt32(c.Name), c.Cache())
            ) ?? ImmutableDictionary<int, IItemSetTemplateEffect>.Empty;
    }
    
    public int ID { get; }
    public int SetCompleteCount { get; }
    
    public ICollection<int> Items { get; }
    public IDictionary<int, IItemSetTemplateEffect> Effects { get; }
}
