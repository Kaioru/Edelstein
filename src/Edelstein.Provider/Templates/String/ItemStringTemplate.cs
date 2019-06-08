using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.String
{
    public class ItemStringTemplate : IStringTemplate
    {
        public int ID { get; }
        public string Name { get; }

        public ItemStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
        }
    }
}