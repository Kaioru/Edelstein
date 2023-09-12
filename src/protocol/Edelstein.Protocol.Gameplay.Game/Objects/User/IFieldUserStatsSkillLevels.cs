namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUserStatsSkillLevels
{
    int this[int skillID] { get; }
    IDictionary<int, int> Records { get; }
}
