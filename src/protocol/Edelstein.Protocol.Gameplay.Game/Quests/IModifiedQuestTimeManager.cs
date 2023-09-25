using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IModifiedQuestTimeManager :
    IRepositoryMethodInsert<int, IModifiedQuestTime>,
    IRepositoryMethodRetrieve<int, IModifiedQuestTime>,
    IRepositoryMethodRetrieveAll<int, IModifiedQuestTime>,
    IRepositoryMethodDelete<int, IModifiedQuestTime>
{
}
