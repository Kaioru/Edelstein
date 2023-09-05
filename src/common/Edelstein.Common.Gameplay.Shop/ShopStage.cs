using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Common.Gameplay.Shop;

public class ShopStage : AbstractStage<IShopStageUser>, IShopStage
{
    public ShopStage(string id) => ID = id;
    
    public override string ID { get; }

    public async Task Enter(IShopStageUser user)
    {
        Console.WriteLine("Entered shop!");
        await base.Enter(user);
    }
}
