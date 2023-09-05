using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Shop;

public interface IShopStageOptions : IIdentifiable<string>
{
    int WorldID { get; }
}
