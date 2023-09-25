namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateActSkill
{
    int SkillID { get; }
    int SkillLevel { get; }
    int MasterLevel { get; }
    bool IsOnlyMasterLevel { get; }
    ICollection<int> Jobs { get; }
}
