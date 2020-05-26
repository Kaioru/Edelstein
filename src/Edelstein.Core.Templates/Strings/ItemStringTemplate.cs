using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Strings
{
    public class ItemStringTemplate : IDataStringTemplate
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