namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateActSP
{
    int SP { get; }
    ICollection<int> Jobs { get; }
}
