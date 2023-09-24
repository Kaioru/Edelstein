namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateAct
{
    int? IncEXP { get; }
    int? IncMoney { get; }
    
    int? NextQuest { get; }
}
