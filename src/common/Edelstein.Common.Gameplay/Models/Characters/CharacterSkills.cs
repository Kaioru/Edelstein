using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills;

namespace Edelstein.Common.Gameplay.Models.Characters;

public class CharacterSkills : ICharacterSkills
{
    public ISkillRecord? this[int skillID] => Records.TryGetValue(skillID, out var record) ? record : null;
    public IDictionary<int, ISkillRecord> Records { get; } = new Dictionary<int, ISkillRecord>();
}
