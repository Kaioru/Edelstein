﻿using System.Collections.Immutable;
using Edelstein.Protocol.Data;
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
    
    public SkillTemplate(int id, IDataProperty property)
    {
        ID = id;

        IsPSD = (property.Resolve<int>("psd") ?? 0) > 0;
        IsPrepared = property.Resolve("prepare") != null;
        IsSummon = property.Resolve("summon") != null;
        IsInvisible = (property.Resolve<int>("invisible") ?? 0) > 0;
        IsCombatOrders = (property.Resolve<int>("combatOrders") ?? 0) > 0;

        var elemAttr = property.ResolveOrDefault<string>("elemAttr") ?? string.Empty;

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

        Delay = property.Resolve("effect")?.Children.Sum(c => c.Resolve<int>("delay") ?? 0) ?? 0;

        PsdSkill = property.Resolve("psdSkill")?
            .Select(c => Convert.ToInt32(c.Name))
            .ToImmutableList() ?? ImmutableList<int>.Empty;
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
                .Range(1, maxLevel + (IsCombatOrders ? 2 : 0))
                .ToImmutableDictionary(
                    i => i,
                    i => (ISkillTemplateLevel)new SkillTemplateLevelCommon(i, common.ResolveAll())
                );
            MaxLevel = (short)maxLevel;
        }
        else
        {
            var level = property.Resolve("level");

            if (level != null)
                Levels = level.Children.ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => (ISkillTemplateLevel)new SkillTemplateLevel(Convert.ToInt32(c.Name), c.ResolveAll())
                );
            else
                Levels = new Dictionary<int, ISkillTemplateLevel>();
            MaxLevel = (short)(Levels?.Count ?? 0);
        }
    }
}
