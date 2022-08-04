using Edelstein.Protocol.Util.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continents;

public interface IContiMoveManager :
    IRepositoryMethodInsert<int, IContiMove>,
    IRepositoryMethodRetrieve<int, IContiMove>
{
    Task<IContiMove?> RetrieveByName(string name);
    Task<IContiMove?> RetrieveByField(IField field);
}
