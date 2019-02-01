using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.String
{
    public class ItemStringTemplate : IStringTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        
        public static ItemStringTemplate Parse(int id, IDataProperty property)
        {
            var t = new ItemStringTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Name = p.ResolveOrDefault<string>("name") ?? "NO-NAME";
                //t.Desc = p.ResolveOrDefault<string>("desc");
            });
            return t;
        }
    }
}