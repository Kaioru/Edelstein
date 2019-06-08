namespace Edelstein.Provider.Templates.String
{
    public class FieldStringTemplate : IStringTemplate
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