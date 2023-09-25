using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplate : ITemplate
{
    string Name { get; }
    
    bool IsAutoAccept { get; }
    bool IsAutoStart { get; }
    bool IsAutoComplete { get; }
    bool IsAutoPreComplete { get; }
    
    IQuestTemplateAct ActStart { get; }
    IQuestTemplateAct ActEnd { get; }
    
    IQuestTemplateCheck CheckStart { get; }
    IQuestTemplateCheck CheckEnd { get; }
}
