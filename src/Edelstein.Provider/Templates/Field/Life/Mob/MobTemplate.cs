namespace Edelstein.Provider.Templates.Field.Life.Mob
{
    public class MobTemplate : ITemplate
    {
        public int ID { get; }

        public MoveAbilityType MoveAbility { get; }

        public short Level { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }

        public MobTemplate(int id, IDataProperty property)
        {
            ID = id;

            if (property.Resolve("fly") != null) MoveAbility = MoveAbilityType.Fly;
            else if (property.Resolve("jump") != null) MoveAbility = MoveAbilityType.Jump;
            else if (property.Resolve("move") != null) MoveAbility = MoveAbilityType.Walk;
            else MoveAbility = MoveAbilityType.Stop;

            Level = property.Resolve<short>("info/level") ?? 0;
            MaxHP = property.Resolve<int>("info/maxHP") ?? 1;
            MaxMP = property.Resolve<int>("info/maxMP") ?? 0;
        }
    }
}