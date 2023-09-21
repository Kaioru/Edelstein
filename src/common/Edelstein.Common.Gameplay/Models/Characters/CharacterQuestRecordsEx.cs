using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterQuestRecordsEx : ICharacterQuestRecordsEx
{
    public IQuestRecordEx? this[int questID] => Records.TryGetValue(questID, out var record) ? record : null;
    public IDictionary<int, IQuestRecordEx> Records { get; } = new Dictionary<int, IQuestRecordEx>();
}
