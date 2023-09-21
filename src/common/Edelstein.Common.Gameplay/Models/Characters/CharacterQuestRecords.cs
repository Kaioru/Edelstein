using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterQuestRecords : ICharacterQuestRecords
{
    public IQuestRecord? this[int questID] => Records.TryGetValue(questID, out var record) ? record : null;
    public IDictionary<int, IQuestRecord> Records { get; } = new Dictionary<int, IQuestRecord>();
}
