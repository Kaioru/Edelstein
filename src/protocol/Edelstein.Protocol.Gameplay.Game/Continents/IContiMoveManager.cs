using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Continents;

public interface IContiMoveManager :
    IRepositoryMethodInsert<string, IContiMove>,
    IRepositoryMethodRetrieve<string, IContiMove>,
    IRepositoryMethodRetrieveAll<string, IContiMove>
{
    Task<IContiMove?> RetrieveByField(IField field);
}
