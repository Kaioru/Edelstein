using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUserStatsSkillLevels : IFieldUserStatsSkillLevels
{
    public int this[int skillID] => Records.TryGetValue(skillID, out var record) ? record : 0;
    public IDictionary<int, int> Records { get; } = new Dictionary<int, int>();
}
