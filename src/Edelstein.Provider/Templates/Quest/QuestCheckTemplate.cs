using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Quest
{
    public class QuestCheckTemplate : QuestOperationTemplate
    {
        public string StartScript { get; }
        public string EndScript { get; }

        public QuestCheckTemplate(int id, IDataProperty property) : base(id, property)
        {
            StartScript = property.ResolveOrDefault<string>("startscript");
            EndScript = property.ResolveOrDefault<string>("endscript");
        }
    }
}