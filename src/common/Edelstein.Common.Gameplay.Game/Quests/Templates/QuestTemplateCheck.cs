using System.Collections.Immutable;
using System.Globalization;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheck : IQuestTemplateCheck
{
    public QuestTemplateCheck(IDataProperty? property)
    {
        ScriptStart = property?.ResolveOrDefault<string>("startscript");
        ScriptEnd = property?.ResolveOrDefault<string>("endscript");
        
        WorldMin = property?.Resolve<int>("worldmin");
        WorldMax = property?.Resolve<int>("worldmax");

        TamingMobLevelMin = property?.Resolve<int>("tamingmoblevelmin");
        TamingMobLevelMax = property?.Resolve<int>("tamingmoblevelmax");
        PetTamenessMin = property?.Resolve<int>("pettamenessmin");
        PetTamenessMax = property?.Resolve<int>("pettamenessmax");

        LevelMin = property?.Resolve<int>("lvmin");
        LevelMax = property?.Resolve<int>("lvmax");
        
        POP = property?.Resolve<int>("pop");

        var start = property?.ResolveOrDefault<string>("start");
        var end = property?.ResolveOrDefault<string>("end");

        if (start != null) DateStart = DateTime.ParseExact(start[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
        if (end != null) DateEnd = DateTime.ParseExact(end[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);

        Jobs = property?.Resolve("job")?.Children
            .Select(c => c.Resolve<int>() ?? -1)
            .ToImmutableList();
        SubJobFlags = property?.Resolve<int>("subJobFlags");

        CheckItem = property?.Resolve("item")?.Children
            .Select(p => (IQuestTemplateCheckItem)new QuestTemplateCheckItem(p.ResolveAll()))
            .ToImmutableList();
        CheckMob = property?.Resolve("mob")?.Children
            .Select(p => (IQuestTemplateCheckMob)new QuestTemplateCheckMob(Convert.ToInt32(p.Name), p.ResolveAll()))
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
