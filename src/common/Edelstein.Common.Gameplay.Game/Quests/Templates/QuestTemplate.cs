using Edelstein.Protocol.Data;
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

    public QuestTemplate(int id, IDataProperty? info, IDataProperty? act, IDataProperty? check)
    {
        ID = id;

        Name = info?.ResolveOrDefault<string>("name") ?? "NO-NAME";
            
        IsAutoAccept = (info?.Resolve<int>("autoAccept") ?? 0) > 0;
        IsAutoStart = (info?.Resolve<int>("autoStart") ?? 0) > 0;
        IsAutoComplete = (info?.Resolve<int>("autoComplete") ?? 0) > 0;
        IsAutoPreComplete = (info?.Resolve<int>("autoPreComplete") ?? 0) > 0;

        ActStart = new QuestTemplateAct(act?.Resolve("0")?.ResolveAll());
        ActEnd = new QuestTemplateAct(act?.Resolve("1")?.ResolveAll());

        CheckStart = new QuestTemplateCheck(check?.Resolve("0")?.ResolveAll());
        CheckEnd = new QuestTemplateCheck(check?.Resolve("1")?.ResolveAll());
    }
}
