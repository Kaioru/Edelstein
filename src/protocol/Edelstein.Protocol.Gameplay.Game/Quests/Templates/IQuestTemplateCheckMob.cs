namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateCheckMob
{
    int Order { get; }
    
    int MobID { get; }
    int Count { get; }
}
