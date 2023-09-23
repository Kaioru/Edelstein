using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Quests.Templates;

public record QuestTemplate : IQuestTemplate
{
    public int ID { get; }
    
    public bool IsAutoAccept { get; }
    public bool IsAutoStart { get; }
    public bool IsAutoComplete { get; }
    public bool IsAutoPreComplete { get; }
    
    public IQuestTemplateAct ActStart { get; }
    public IQuestTemplateAct ActEnd { get; }
    public IQuestTemplateCheck CheckStart { get; }
    public IQuestTemplateCheck CheckEnd { get; }

    public QuestTemplate(int id, IDataProperty property)
    {
        ID = id;

        IsAutoAccept = (property.Resolve<int>("autoAccept") ?? 0) > 0;
        IsAutoStart = (property.Resolve<int>("autoStart") ?? 0) > 0;
        IsAutoComplete = (property.Resolve<int>("autoComplete") ?? 0) > 0;
        IsAutoPreComplete = (property.Resolve<int>("autoPreComplete") ?? 0) > 0;

        ActStart = new QuestTemplateAct(property.Resolve("Act/0"));
        ActEnd = new QuestTemplateAct(property.Resolve("Act/1"));
        
        CheckStart = new QuestTemplateCheck(property.Resolve("Check/0"));
        CheckEnd = new QuestTemplateCheck(property.Resolve("Check/1"));
    }
}
