using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IModifiedQuestTime : IIdentifiable<int>
{
    DateTime DateStart { get; }
    DateTime DateEnd { get; }
}
