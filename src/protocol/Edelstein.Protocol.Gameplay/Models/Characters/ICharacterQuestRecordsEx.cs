using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterQuestRecordsEx
{
    IQuestRecordEx? this[int questID] { get; }
    IDictionary<int, IQuestRecordEx> Records { get; }
}
