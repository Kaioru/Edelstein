using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public record QuestTemplateActSkill : IQuestTemplateActSkill
{
    public QuestTemplateActSkill(IDataNode node)
    {
        SkillID = node.ResolveInt("id") ?? 0;
        SkillLevel = node.ResolveInt("skillLevel") ?? 0;
        MasterLevel = node.ResolveInt("masterLevel") ?? 0;
        IsOnlyMasterLevel = node.ResolveInt("onlyMasterLevel") > 0;
        Jobs = node.ResolvePath("job")?.Children
            .Select(p => p.ResolveInt())
            .Where(i => i.HasValue)
            .Select(i => i.Value)
            .ToFrozenSet();
    }
    
    public int SkillID { get; }
    public int SkillLevel { get; }
    public int MasterLevel { get; }
    public bool IsOnlyMasterLevel { get; }
    public ICollection<int> Jobs { get; }
}
