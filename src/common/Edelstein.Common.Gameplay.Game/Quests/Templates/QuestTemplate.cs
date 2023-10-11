using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplate : IQuestTemplate
{
    public int ID { get; }

    public string Name { get; }
    
    public bool IsAutoAccept { get; }
    public bool IsAutoStart { get; }
    public bool IsAutoComplete { get; }
    public bool IsAutoPreComplete { get; }
    
    public IQuestTemplateAct ActStart { get; }
    public IQuestTemplateAct ActEnd { get; }
    public IQuestTemplateCheck CheckStart { get; }
    public IQuestTemplateCheck CheckEnd { get; }

    public QuestTemplate(int id, IDataNode? info, IDataNode? act, IDataNode? check)
    {
        ID = id;

        Name = info?.ResolveString("name") ?? "NO-NAME";
            
        IsAutoAccept = (info?.ResolveInt("autoAccept") ?? 0) > 0;
        IsAutoStart = (info?.ResolveInt("autoStart") ?? 0) > 0;
        IsAutoComplete = (info?.ResolveInt("autoComplete") ?? 0) > 0;
        IsAutoPreComplete = (info?.ResolveInt("autoPreComplete") ?? 0) > 0;

        ActStart = new QuestTemplateAct(act?.ResolvePath("0")?.ResolveAll());
        ActEnd = new QuestTemplateAct(act?.ResolvePath("1")?.ResolveAll());

        CheckStart = new QuestTemplateCheck(check?.ResolvePath("0")?.ResolveAll());
        CheckEnd = new QuestTemplateCheck(check?.ResolvePath("1")?.ResolveAll());
    }
}
