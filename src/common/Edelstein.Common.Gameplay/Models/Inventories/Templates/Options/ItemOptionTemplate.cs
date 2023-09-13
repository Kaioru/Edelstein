using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Options;

public record ItemOptionTemplate : IItemOptionTemplate
{
    public ItemOptionTemplate(int id, IDataProperty property)
    {
        ID = id;

        var info = property.Resolve("info");
        var level = property.Resolve("level");
        
        Grade = (ItemOptionGrade)(id / 10000);
        Type = (ItemOptionType)(info?.Resolve<short>("optionType") ?? 0);
        
        ReqLevel = info?.Resolve<short>("reqLevel") ?? 0;

        if (level != null)
            Levels = level.Children
                .ToImmutableDictionary(
                    l => Convert.ToInt32(l.Name),
                    l => (IItemOptionTemplateLevel)new ItemOptionTemplateLevel(Convert.ToInt32(l.Name), l.ResolveAll())
                );
        else 
            Levels = new Dictionary<int, IItemOptionTemplateLevel>();
    }
    
    public int ID { get; }
    
    public ItemOptionGrade Grade { get; }
    public ItemOptionType Type { get; }
    
    public short ReqLevel { get; }
    
    public IDictionary<int, IItemOptionTemplateLevel> Levels { get; }
}
