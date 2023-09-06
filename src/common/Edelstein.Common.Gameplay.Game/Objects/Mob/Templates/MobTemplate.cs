using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobTemplate : IMobTemplate
{
    public MobTemplate(int id, IDataProperty property, IDataProperty info)
    {
        ID = id;

        if (property.Resolve("fly") != null) MoveAbility = MobMoveAbilityType.Fly;
        else if (property.Resolve("jump") != null) MoveAbility = MobMoveAbilityType.Jump;
        else if (property.Resolve("move") != null) MoveAbility = MobMoveAbilityType.Walk;
        else MoveAbility = MobMoveAbilityType.Stop;

        Level = info.Resolve<short>("level") ?? 0;

        IsBoss = (info.Resolve<int>("boss") ?? 0) > 0;

        MaxHP = info.Resolve<int>("maxHP") ?? 1;
        MaxMP = info.Resolve<int>("maxMP") ?? 0;

        PAD = info.Resolve<int>("PADamage") ?? 0;
        PDD = info.Resolve<int>("PDDamage") ?? 0;
        PDR = info.Resolve<int>("PDRate") ?? 0;
        MAD = info.Resolve<int>("MADamage") ?? 0;
        MDD = info.Resolve<int>("PDDamage") ?? 0;
        MDR = info.Resolve<int>("MDRate") ?? 0;
        ACC = info.Resolve<int>("acc") ?? 0;
        EVA = info.Resolve<int>("eva") ?? 0;

        EXP = info.Resolve<int>("exp") ?? 0;

        ElementAttributes = new Dictionary<Element, ElementAttribute>
        {
            { Element.Physical, ElementAttribute.None },
            { Element.Ice, ElementAttribute.None },
            { Element.Fire, ElementAttribute.None },
            { Element.Light, ElementAttribute.None },
            { Element.Holy, ElementAttribute.None },
            { Element.Dark, ElementAttribute.None },
            { Element.Undead, ElementAttribute.None }
        };

        var elemCount = 0;
        var elemAttrs = info.ResolveOrDefault<string>("elemAttr") ?? string.Empty;
        
        foreach (var group in elemAttrs.GroupBy(_ => elemCount++ / 2).ToList())
        {
            var groupList = group.ToList();
            var elem = groupList[0] switch
            {
                'P' => Element.Physical,
                'I' => Element.Ice,
                'F' => Element.Fire,
                'L' => Element.Light,
                'H' => Element.Holy,
                'D' => Element.Dark,
                'S' => Element.Undead,
                _ => Element.Physical
            };
            var elemAttr = (ElementAttribute)Convert.ToInt32(groupList[1].ToString());

            ElementAttributes[elem] = elemAttr;
        }
    }
    
    public int ID { get; }

    public MobMoveAbilityType MoveAbility { get; }

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
