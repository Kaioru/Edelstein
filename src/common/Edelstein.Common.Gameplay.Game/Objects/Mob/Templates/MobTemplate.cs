using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobTemplate : IMobTemplate
{
    public int ID { get; }

    public MobMoveAbilityType MoveAbility { get; }

    public short Level { get; }

    public int MaxHP { get; }
    public int MaxMP { get; }

    public int PAD { get; }
    public int PDR { get; }
    public int MAD { get; }
    public int MDR { get; }
    public int ACC { get; }
    public int EVA { get; }

    public int EXP { get; }
    
    public MobTemplate(int id, IDataProperty property, IDataProperty info)
    {
        ID = id;

        if (property.Resolve("fly") != null) MoveAbility = MobMoveAbilityType.Fly;
        else if (property.Resolve("jump") != null) MoveAbility = MobMoveAbilityType.Jump;
        else if (property.Resolve("move") != null) MoveAbility = MobMoveAbilityType.Walk;
        else MoveAbility = MobMoveAbilityType.Stop;

        Level = info.Resolve<short>("level") ?? 0;

        MaxHP = info.Resolve<int>("maxHP") ?? 1;
        MaxMP = info.Resolve<int>("maxMP") ?? 0;

        PAD = info.Resolve<int>("pad") ?? 0;
        PDR = info.Resolve<int>("pdr") ?? 10;
        MAD = info.Resolve<int>("mad") ?? 0;
        MDR = info.Resolve<int>("mdr") ?? 10;
        ACC = info.Resolve<int>("acc") ?? 0;
        EVA = info.Resolve<int>("eva") ?? 0;

        EXP = info.Resolve<int>("exp") ?? 0;
    }

}
