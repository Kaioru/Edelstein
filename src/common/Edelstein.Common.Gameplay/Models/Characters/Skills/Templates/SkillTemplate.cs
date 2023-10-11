using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplate : ISkillTemplate
{
    public int ID { get; }

    public ISkillTemplateLevel? this[int level] => Levels.TryGetValue(level, out var result) ? result : null;

    public short MaxLevel { get; }
    
    public bool IsPSD { get; }
    public bool IsPrepared { get; }
    public bool IsSummon { get; }
    public bool IsInvisible { get; }
    public bool IsCombatOrders { get; }
    
    public Element Element { get; }
    
    public int Delay { get; }
    
    public ICollection<int> PsdSkill { get; }
    public IDictionary<int, int> ReqSkill { get; }
    public IDictionary<int, ISkillTemplateLevel> Levels { get; }
    
    public SkillTemplate(int id, IDataNode node)
    {
        ID = id;

        IsPSD = (node.ResolveInt("psd") ?? 0) > 0;
        IsPrepared = node.ResolvePath("prepare") != null;
        IsSummon = node.ResolvePath("summon") != null;
        IsInvisible = (node.ResolveInt("invisible") ?? 0) > 0;
        IsCombatOrders = (node.ResolveInt("combatOrders") ?? 0) > 0;

        var elemAttr = node.ResolveString("elemAttr") ?? string.Empty;

        Element = Element.Physical;
        if (elemAttr.Length > 0)
            Element = elemAttr.ToUpper()[0] switch
            {
                'P' => Element.Physical,
                'I' => Element.Ice,
                'F' => Element.Fire,
                'L' => Element.Light,
                'S' => Element.Poison,
                'H' => Element.Holy,
                'D' => Element.Dark,
                'U' => Element.Undead,
                _ => Element.Physical
            };

        Delay = node.ResolvePath("effect")?.Children.Sum(c => c.ResolveInt("delay") ?? 0) ?? 0;

        PsdSkill = node.ResolvePath("psdSkill")?
            .Select(c => Convert.ToInt32(c.Name))
            .ToImmutableList() ?? ImmutableList<int>.Empty;
        ReqSkill = node.ResolvePath("req")?.Children
            .ToImmutableDictionary(
                c => Convert.ToInt32(c.Name),
                c => c.ResolveInt() ?? 0
            ) ?? ImmutableDictionary<int, int>.Empty;

        var common = node.ResolvePath("common");

        if (common != null)
        {
            var maxLevel = common.ResolveInt("maxLevel") ?? 0;
            
            Levels = Enumerable
                .Range(1, maxLevel + (IsCombatOrders ? 2 : 0))
                .ToImmutableDictionary(
                    i => i,
                    i => (ISkillTemplateLevel)new SkillTemplateLevelCommon(i, common.Cache())
                );
            MaxLevel = (short)maxLevel;
        }
        else
        {
            var level = node.ResolvePath("level");

            Levels = level?.Children.ToImmutableDictionary(
                c => Convert.ToInt32(c.Name),
                c => (ISkillTemplateLevel)new SkillTemplateLevel(Convert.ToInt32(c.Name), c.Cache())
            ) ?? ImmutableDictionary<int, ISkillTemplateLevel>.Empty;
            MaxLevel = (short)(Levels?.Count ?? 0);
        }
    }
}
