namespace Edelstein.Provider.Templates.Field.Life.Mob
{
    public class MobTemplate : ITemplate
    {
        public int ID { get; }

        public MoveAbilityType MoveAbility { get; set; }

        public short Level { get; set; }
        public int EXP { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public MobTemplate(int id, IDataProperty property)
        {
            ID = id;

            if (property.Resolve("fly") != null) MoveAbility = MoveAbilityType.Fly;
            else if (property.Resolve("jump") != null) MoveAbility = MoveAbilityType.Jump;
            else if (property.Resolve("move") != null) MoveAbility = MoveAbilityType.Walk;
            else MoveAbility = MoveAbilityType.Stop;

            Level = property.Resolve<short>("info/level") ?? 0;
            EXP = property.Resolve<int>("info/exp") ?? 0;
            MaxHP = property.Resolve<int>("info/maxHP") ?? 1;
            MaxMP = property.Resolve<int>("info/maxMP") ?? 0;
        }
    }
}