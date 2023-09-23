using System.Globalization;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Quests.Templates;

public record QuestTemplateCheck : IQuestTemplateCheck
{
    public QuestTemplateCheck(IDataProperty? property)
    {
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

        Job = property?.Resolve<int>("job");
        SubJobFlags = property?.Resolve<int>("subJobFlags");
    }
    
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
    
    public int? Job { get; }
    public int? SubJobFlags { get; }
}
