namespace Edelstein.Provider.Templates.String
{
    public class QuestStringTemplate : IStringTemplate
    {
        public int ID { get; }
        public string Name { get; }
        
        public QuestStringTemplate(int id, IDataProperty property)
        {
            ID = id;
            
            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
        }
    }
}