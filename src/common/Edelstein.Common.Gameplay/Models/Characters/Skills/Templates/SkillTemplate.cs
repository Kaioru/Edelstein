using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplate : ISkillTemplate
{
    public int ID { get; }

    public ISkillTemplateLevel? this[int level] => Levels.Retrieve(level).Result;

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
    public ITemplateCollection<ISkillTemplateLevel> Levels { get; }
    
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
            .ToFrozenSet() ?? FrozenSet<int>.Empty;
        ReqSkill = node.ResolvePath("req")?.Children
            .ToFrozenDictionary(
                c => Convert.ToInt32(c.Name),
                c => c.ResolveInt() ?? 0
            ) ?? FrozenDictionary<int, int>.Empty;

        var common = node.ResolvePath("common");

        if (common != null)
        {
            var maxLevelNode = common.ResolvePath("maxLevel");
            var maxLevel = maxLevelNode?.ResolveInt() ?? 0;
            var maxLevelStr = maxLevelNode?.ResolveString();

            // Fixes Advanced Yellow Aura
            if (maxLevelStr != null)
                maxLevel = Convert.ToInt32(maxLevelStr);
            
            Levels = new TemplateCollectionProvider<ISkillTemplateLevel>(Enumerable
                .Range(1, maxLevel + (IsCombatOrders ? 2 : 0))
                .ToFrozenDictionary(
                    i => i,
                    i => (ITemplateProvider<ISkillTemplateLevel>)new TemplateProviderLazy<ISkillTemplateLevel>(
                        i,
                        () => new SkillTemplateLevelCommon(i, common.Cache())
                    )
                ));
            MaxLevel = (short)maxLevel;
        }
        else
        {
            var level = node.ResolvePath("level");

            Levels = new TemplateCollectionProvider<ISkillTemplateLevel>(
                level?.Children.ToFrozenDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => (ITemplateProvider<ISkillTemplateLevel>)new TemplateProviderEager<ISkillTemplateLevel>(
                        Convert.ToInt32(c.Name),
                        new SkillTemplateLevel(Convert.ToInt32(c.Name), c.Cache()))
                    ) ?? FrozenDictionary<int, ITemplateProvider<ISkillTemplateLevel>>.Empty
                );
            MaxLevel = (short)(Levels?.Count ?? 0);
        }
    }
}
