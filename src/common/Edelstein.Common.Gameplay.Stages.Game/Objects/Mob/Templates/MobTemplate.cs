using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates
{
    public record MobTemplate : ITemplate, IFieldObjMobInfo
    {
        public int ID { get; }

        public MoveAbilityType MoveAbility { get; }

        public short Level { get; }
        public int EXP { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }

        public MobTemplate(int id, IDataProperty property, IDataProperty info)
        {
            ID = id;

            if (property.Resolve("fly") != null) MoveAbility = MoveAbilityType.Fly;
            else if (property.Resolve("jump") != null) MoveAbility = MoveAbilityType.Jump;
            else if (property.Resolve("move") != null) MoveAbility = MoveAbilityType.Walk;
            else MoveAbility = MoveAbilityType.Stop;

            Level = info.Resolve<short>("level") ?? 0;
            EXP = info.Resolve<int>("exp") ?? 0;
            MaxHP = info.Resolve<int>("maxHP") ?? 1;
            MaxMP = info.Resolve<int>("maxMP") ?? 0;
        }
    }
}
