namespace Edelstein.Provider.Templates.Quest
{
    public class QuestActTemplate : QuestOperationTemplate
    {
        public int EXP { get; }

        public QuestActTemplate(int id, IDataProperty property) : base(id, property)
        {
            EXP = property.Resolve<int>("exp") ?? 0;
        }
    }
}