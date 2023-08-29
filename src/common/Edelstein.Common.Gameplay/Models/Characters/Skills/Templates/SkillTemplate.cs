using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplate : ISkillTemplate
{
    public int ID { get; }
    
    public short MaxLevel { get; }
    
    public bool IsPSD { get; }
    public bool IsSummon { get; }
    public bool IsInvisible { get; }
    
    public IDictionary<int, int> ReqSkill { get; }
    public IDictionary<int, ISkillTemplateLevel> Levels { get; }
    
    public SkillTemplate(int id, IDataProperty property)
    {
        ID = id;

        IsPSD = (property.Resolve<int>("psd") ?? 0) > 0;
        IsSummon = property.Resolve("summon") != null;
        IsInvisible = (property.Resolve<int>("invisible") ?? 0) > 0;

        ReqSkill = property.Resolve("req")?.Children
            .ToImmutableDictionary(
                c => Convert.ToInt32(c.Name),
                c => c.Resolve<int>() ?? 0
            ) ?? ImmutableDictionary<int, int>.Empty;

        var common = property.Resolve("common");

        if (common != null)
        {
            var maxLevel = common.Resolve<int>("maxLevel") ?? 0;

            Levels = Enumerable
                .Range(1, maxLevel)
                .ToImmutableDictionary(
                    i => i,
                    i => (ISkillTemplateLevel)new SkillTemplateLevelCommon(i, common.ResolveAll())
                );
        }
        else
        {
            var level = property.Resolve("level");
            
            if (level != null)
                Levels = level.Children.ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => (ISkillTemplateLevel)new SkillTemplateLevel(c.ResolveAll())
                );
        }

        MaxLevel = (short)Levels.Count;
    }
}
