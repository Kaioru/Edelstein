using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.String
{
    public class FieldStringTemplate : IStringTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StreetName { get; set; }

        public static FieldStringTemplate Parse(int id, IDataProperty property)
        {
            var t = new FieldStringTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Name = p.ResolveOrDefault<string>("mapName") ?? "NO-NAME";
                t.StreetName = p.ResolveOrDefault<string>("streetName");
            });
            return t;
        }
    }
}