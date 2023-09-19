using Edelstein.Protocol.Gameplay.Shop.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Shop;

public class ShopStageUserInitializer : IAdapterInitializer
{
    private readonly ShopContext _context;

    public ShopStageUserInitializer(ShopContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new ShopStageUser(socket, _context);
}
