using Edelstein.Provider;

namespace Edelstein.Core.Templates.Strings
{
    public class FieldStringTemplate : IDataStringTemplate
    {
        public int ID { get; }
        public string Name => MapName;

        public string MapName { get; }
        public string StreetName { get; }

        public FieldStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            MapName = property.ResolveOrDefault<string>("mapName") ?? "NO-NAME";
            StreetName = property.ResolveOrDefault<string>("streetName");
        }
    }
}