using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterQuestRecords
{
    IQuestRecord? this[int questID] { get; }
    IDictionary<int, IQuestRecord> Records { get; }
}
