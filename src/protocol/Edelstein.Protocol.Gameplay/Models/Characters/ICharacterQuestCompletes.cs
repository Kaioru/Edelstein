using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterQuestCompletes
{
    IQuestCompleteRecord? this[int questID] { get; }
    IDictionary<int, IQuestCompleteRecord> Records { get; }
}
