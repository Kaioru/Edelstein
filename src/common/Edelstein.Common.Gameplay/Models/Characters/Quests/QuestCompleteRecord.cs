using Edelstein.Protocol.Gameplay.Models.Characters.Quests;

namespace Edelstein.Common.Gameplay.Models.Characters.Quests;

public record QuestCompleteRecord : IQuestCompleteRecord
{
    public DateTime DateFinish { get; set; }
}
