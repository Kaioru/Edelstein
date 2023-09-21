using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterQuestCompletes : ICharacterQuestCompletes
{
    public IQuestCompleteRecord? this[int questID] => Records.TryGetValue(questID, out var record) ? record : null;
    public IDictionary<int, IQuestCompleteRecord> Records { get; } = new Dictionary<int, IQuestCompleteRecord>();
}
