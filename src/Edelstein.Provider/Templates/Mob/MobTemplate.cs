namespace Edelstein.Provider.Templates.Mob
{
    public class MobTemplate : ITemplate
    {
        public int ID { get; }

        public short Level { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public MobTemplate(int id, IDataProperty property)
        {
            ID = id;

            Level = property.Resolve<short>("info/level") ?? 0;
            MaxHP = property.Resolve<int>("info/maxHP") ?? 1;
            MaxMP = property.Resolve<int>("info/maxMP") ?? 0;
        }
    }
}