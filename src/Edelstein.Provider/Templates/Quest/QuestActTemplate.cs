namespace Edelstein.Provider.Templates.Quest
{
    public class QuestActTemplate : ITemplate
    {
        public int ID { get; }
        
        public QuestActTemplate(int id, IDataProperty property)
        {
            ID = id;
        }
    }
}