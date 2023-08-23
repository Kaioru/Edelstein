using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Continents;

public interface IContiMoveManager :
    IRepositoryMethodInsert<int, IContiMove>,
    IRepositoryMethodRetrieve<int, IContiMove>,
    IRepositoryMethodRetrieveAll<int, IContiMove>
{
    Task<IContiMove?> RetrieveByName(string name);
    Task<IContiMove?> RetrieveByField(IField field);
}
