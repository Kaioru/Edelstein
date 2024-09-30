using System;
using System.Collections.Immutable;
using System.Linq;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates.Options;

public record ItemOptionTemplate : IItemOptionTemplate
{
    public int ID { get; }

    public ItemOptionGrade Grade { get; }
    public ItemOptionType Type { get; }

    public short ReqLevel { get; }

    public ITemplateCollection<IItemOptionTemplateLevel> Levels { get; }

    public ItemOptionTemplate(int id, IDataNode node)
    {
        ID = id;
        
        var info = node.ResolvePath("info");
        var level = node.ResolvePath("level");

        Grade = (ItemOptionGrade)(id / 10000);
        Type = (ItemOptionType)(info?.ResolveShort("optionType") ?? 0);

        ReqLevel = info?.ResolveShort("reqLevel") ?? 0;

        Levels = new TemplateCollection<IItemOptionTemplateLevel>(level?.Children
                .Select(l => (level: Convert.ToInt32(l.Name), node: l))
                .ToImmutableDictionary(
                    kv => kv.level,
                    kv => (ITemplateProvider<IItemOptionTemplateLevel>)new TemplateProviderLazy<IItemOptionTemplateLevel>(
                        Convert.ToInt32(kv.level),
                        () => new ItemOptionTemplateLevel(kv.level, kv.node)
                    )
                ) ?? ImmutableDictionary<int, ITemplateProvider<IItemOptionTemplateLevel>>.Empty
        );
    }
}
