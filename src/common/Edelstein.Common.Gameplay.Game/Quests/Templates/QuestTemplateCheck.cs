using System.Collections.Immutable;
using System.Globalization;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheck : IQuestTemplateCheck
{
    public QuestTemplateCheck(IDataNode? node)
    {
        ScriptStart = node?.ResolveString("startscript");
        ScriptEnd = node?.ResolveString("endscript");
        
        WorldMin = node?.ResolveInt("worldmin");
        WorldMax = node?.ResolveInt("worldmax");

        TamingMobLevelMin = node?.ResolveInt("tamingmoblevelmin");
        TamingMobLevelMax = node?.ResolveInt("tamingmoblevelmax");
        PetTamenessMin = node?.ResolveInt("pettamenessmin");
        PetTamenessMax = node?.ResolveInt("pettamenessmax");

        LevelMin = node?.ResolveInt("lvmin");
        LevelMax = node?.ResolveInt("lvmax");
        
        POP = node?.ResolveInt("pop");

        var start = node?.ResolveString("start");
        var end = node?.ResolveString("end");

        if (start != null) DateStart = DateTime.ParseExact(start[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
        if (end != null) DateEnd = DateTime.ParseExact(end[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);

        Jobs = node?.ResolvePath("job")?.Children
            .Select(c => c.ResolveInt() ?? -1)
            .ToImmutableList();
        SubJobFlags = node?.ResolveInt("subJobFlags");

        CheckItem = node?.ResolvePath("item")?.Children
            .Select(p => (IQuestTemplateCheckItem)new QuestTemplateCheckItem(p.Cache()))
            .ToImmutableList();
        CheckMob = node?.ResolvePath("mob")?.Children
            .Select(p => (IQuestTemplateCheckMob)new QuestTemplateCheckMob(Convert.ToInt32(p.Name), p.Cache()))
            .ToImmutableSortedSet(new QuestTemplateCheckMobComparer());
    }

    public string? ScriptStart { get; }
    public string? ScriptEnd { get; }
    
    public int? WorldMin { get; }
    public int? WorldMax { get; }
    
    public int? TamingMobLevelMin { get; }
    public int? TamingMobLevelMax { get; }
    public int? PetTamenessMin { get; }
    public int? PetTamenessMax { get; }
    
    public int? LevelMin { get; }
    public int? LevelMax { get; }
    
    public int? POP { get; }
    
    public DateTime? DateStart { get; }
    public DateTime? DateEnd { get; }
    
    public ICollection<int>? Jobs { get; }
    public int? SubJobFlags { get; }
    
    public ICollection<IQuestTemplateCheckItem>? CheckItem { get; }
    public ICollection<IQuestTemplateCheckMob>? CheckMob { get; }
}
