using System.Collections.Frozen;
using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public record QuestTemplateActSkill : IQuestTemplateActSkill
{
    public QuestTemplateActSkill(IDataProperty property)
    {
        SkillID = property.Resolve<int>("id") ?? 0;
        SkillLevel = property.Resolve<int>("skillLevel") ?? 0;
        MasterLevel = property.Resolve<int>("masterLevel") ?? 0;
        IsOnlyMasterLevel = property.Resolve<int>("onlyMasterLevel") > 0;
        Jobs = property.Resolve("job")?.Children
            .Select(p => p.Resolve<int>())
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
