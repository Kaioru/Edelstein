using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Shop.Commodities;

public interface INotSaleManager :
    IRepositoryMethodInsert<int, INotSale>,
    IRepositoryMethodRetrieve<int, INotSale>,
    IRepositoryMethodRetrieveAll<int, INotSale>,
    IRepositoryMethodDelete<int, INotSale>;
