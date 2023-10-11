using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobTemplate : IMobTemplate
{
    public MobTemplate(int id, IDataNode node, IDataNode info)
    {
        ID = id;

        if (node.ResolvePath("fly") != null) MoveAbility = MoveAbilityType.Fly;
        else if (node.ResolvePath("jump") != null) MoveAbility = MoveAbilityType.Jump;
        else if (node.ResolvePath("move") != null) MoveAbility = MoveAbilityType.Walk;
        else MoveAbility = MoveAbilityType.Stop;

        Level = info.ResolveShort("level") ?? 0;

        IsBoss = (info.ResolveInt("boss") ?? 0) > 0;

        MaxHP = info.ResolveInt("maxHP") ?? 1;
        MaxMP = info.ResolveInt("maxMP") ?? 0;

        PAD = info.ResolveInt("PADamage") ?? 0;
        PDD = info.ResolveInt("PDDamage") ?? 0;
        PDR = info.ResolveInt("PDRate") ?? 0;
        MAD = info.ResolveInt("MADamage") ?? 0;
        MDD = info.ResolveInt("PDDamage") ?? 0;
        MDR = info.ResolveInt("MDRate") ?? 0;
        ACC = info.ResolveInt("acc") ?? 0;
        EVA = info.ResolveInt("eva") ?? 0;

        EXP = info.ResolveInt("exp") ?? 0;

        ElementAttributes = new Dictionary<Element, ElementAttribute>
        {
            { Element.Physical, ElementAttribute.None },
            { Element.Ice, ElementAttribute.None },
            { Element.Fire, ElementAttribute.None },
            { Element.Light, ElementAttribute.None },
            { Element.Poison, ElementAttribute.None },
            { Element.Holy, ElementAttribute.None },
            { Element.Dark, ElementAttribute.None },
            { Element.Undead, ElementAttribute.None }
        };

        var elemCount = 0;
        var elemAttrs = info.ResolveString("elemAttr") ?? string.Empty;
        
        foreach (var group in elemAttrs.GroupBy(_ => elemCount++ / 2).ToList())
        {
            var groupList = group.ToList();
            var elem = groupList[0] switch
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
            var elemAttr = (ElementAttribute)Convert.ToInt32(groupList[1].ToString());

            ElementAttributes[elem] = elemAttr;
        }
    }
    
    public int ID { get; }

    public MoveAbilityType MoveAbility { get; }

    public short Level { get; }
    
    public bool IsBoss { get; }

    public int MaxHP { get; }
    public int MaxMP { get; }

    public int PAD { get; }
    public int PDD { get; }
    public int PDR { get; }
    public int MAD { get; }
    public int MDD { get; }
    public int MDR { get; }
    public int ACC { get; }
    public int EVA { get; }

    public int EXP { get; }
    
    public IDictionary<Element, ElementAttribute> ElementAttributes { get; }
}
