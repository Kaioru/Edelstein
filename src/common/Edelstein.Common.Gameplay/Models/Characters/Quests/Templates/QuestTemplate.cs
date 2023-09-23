﻿using Edelstein.Protocol.Data;
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

        var act = property.Resolve("Act")?.ResolveAll();
        
        ActStart = new QuestTemplateAct(act?.Resolve("0"));
        ActEnd = new QuestTemplateAct(act?.Resolve("1"));

        var check = property.Resolve("Check")?.ResolveAll();
        
        CheckStart = new QuestTemplateCheck(check?.Resolve("Check/0"));
        CheckEnd = new QuestTemplateCheck(check?.Resolve("Check/1"));
    }
}