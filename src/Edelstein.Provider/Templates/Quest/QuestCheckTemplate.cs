namespace Edelstein.Provider.Templates.Quest
{
    public class QuestCheckTemplate : ITemplate
    {
        public int ID { get; }
        
        public QuestCheckTemplate(int id, IDataProperty property)
        {
            ID = id;
        }
    }
}