using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateActSP : IQuestTemplateActSP
{
    public QuestTemplateActSP(IDataNode node)
    {
        SP = node.ResolveInt("sp_value") ?? 0;
        Jobs = node.ResolvePath("job")?.Children
            .Select(p => p.ResolveInt())
            .Where(i => i.HasValue)
            .Select(i => i.Value)
            .ToFrozenSet();
    }
    
    public int SP { get; }
    public ICollection<int> Jobs { get; }
}
