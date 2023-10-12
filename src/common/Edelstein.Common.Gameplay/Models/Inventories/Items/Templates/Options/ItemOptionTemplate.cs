using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates.Options;

public record ItemOptionTemplate : IItemOptionTemplate
{
    public ItemOptionTemplate(int id, IDataNode node)
    {
        ID = id;

        var info = node.ResolvePath("info");
        var level = node.ResolvePath("level");
        
        Grade = (ItemOptionGrade)(id / 10000);
        Type = (ItemOptionType)(info?.ResolveShort("optionType") ?? 0);
        
        ReqLevel = info?.ResolveShort("reqLevel") ?? 0;

        Levels = level?.Children
            .ToImmutableDictionary(
                l => Convert.ToInt32(l.Name),
                l => (IItemOptionTemplateLevel)new ItemOptionTemplateLevel(Convert.ToInt32(l.Name), l.Cache())
            ) ?? ImmutableDictionary<int, IItemOptionTemplateLevel>.Empty;
    }
    
    public int ID { get; }
    
    public ItemOptionGrade Grade { get; }
    public ItemOptionType Type { get; }
    
    public short ReqLevel { get; }
    
    public IDictionary<int, IItemOptionTemplateLevel> Levels { get; }
}
