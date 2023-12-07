using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public interface INPCShopManager :
    IRepositoryMethodInsert<int, INPCShop>,
    IRepositoryMethodRetrieve<int, INPCShop>,
    IRepositoryMethodRetrieveAll<int, INPCShop>,
    IRepositoryMethodDelete<int, INPCShop>;
