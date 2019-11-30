using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.String
{
    public class MobStringTemplate : IStringTemplate
    {
        public int ID { get; }
        public string Name { get; }

        public MobStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
        }
    }
}