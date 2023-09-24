using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateActSP : IQuestTemplateActSP
{
    public QuestTemplateActSP(IDataProperty property)
    {
        SP = property.Resolve<int>("sp_value") ?? 0;
        Jobs = property.Resolve("job")?.Children
            .Select(p => p.Resolve<int>())
            .Where(i => i.HasValue)
            .Select(i => i.Value)
            .ToImmutableList();
    }
    
    public int SP { get; }
    public ICollection<int> Jobs { get; }
}
