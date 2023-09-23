using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;

public interface IQuestTemplate : ITemplate
{
    bool IsAutoAccept { get; }
    bool IsAutoStart { get; }
    bool IsAutoComplete { get; }
    bool IsAutoPreComplete { get; }
    
    IQuestTemplateAct ActStart { get; }
    IQuestTemplateAct ActEnd { get; }
    
    IQuestTemplateCheck CheckStart { get; }
    IQuestTemplateCheck CheckEnd { get; }
}
