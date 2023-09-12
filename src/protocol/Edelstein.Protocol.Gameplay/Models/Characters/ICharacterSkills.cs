using Edelstein.Protocol.Gameplay.Models.Characters.Skills;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterSkills
{
    ISkillRecord? this[int skillID] { get; }
    IDictionary<int, ISkillRecord> Records { get; }
}
