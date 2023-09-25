using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Common.Gameplay.Models.Characters.Quests;

public record QuestRecord : IQuestRecord
{
    public string Value { get; set; }
}
