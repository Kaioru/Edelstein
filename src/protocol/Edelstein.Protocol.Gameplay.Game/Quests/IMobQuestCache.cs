using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IMobQuestCache : IIdentifiable<int>
{
    ICollection<int> Quests { get; }
}
