using Edelstein.Protocol.Gameplay.Shop.Contexts;

namespace Edelstein.Protocol.Gameplay.Shop;

public interface IShopStageUser : IStageUser<IShopStageUser>
{
    ShopContext Context { get; }
    
    string? FromServerID { get; set; }
}
