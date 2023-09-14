using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Shop.Commodities;

public interface IModifiedCommodityManager :
    IRepositoryMethodInsert<int, IModifiedCommodity>,
    IRepositoryMethodRetrieve<int, IModifiedCommodity>,
    IRepositoryMethodRetrieveAll<int, IModifiedCommodity>,
    IRepositoryMethodDelete<int, IModifiedCommodity>
{
}
