using Edelstein.Protocol.Gameplay.Game.Quests;

namespace Edelstein.Common.Gameplay.Game.Quests;

public record MobQuestCache(int ID) : IMobQuestCache
{
    public ICollection<int> Quests { get; } = new HashSet<int>();
}
