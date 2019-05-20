namespace Edelstein.Provider.Templates.Quest
{
    public class QuestCheckTemplate : ITemplate
    {
        public int ID { get; }
        
        public string? StartScript { get; set; }
        public string? EndScript { get; set; }
        
        public QuestCheckTemplate(int id, IDataProperty property)
        {
            ID = id;

            StartScript = property.ResolveOrDefault<string>("startscript");
            EndScript = property.ResolveOrDefault<string>("endscript");
        }
    }
}