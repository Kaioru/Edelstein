using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Shop;

public class ShopStageUser : AbstractStageUser<IShopStageUser>, IShopStageUser
{
    public ShopStageUser(ISocket socket, ShopContext context) : base(socket) 
        => Context = context;

    public ShopContext Context { get; }
    public string? FromServerID { get; set; }

    public override Task Migrate(string serverID, IPacket? packet = null)
        => Context.Pipelines.UserMigrate.Process(new UserMigrate<IShopStageUser>(this, serverID, packet));

    public override Task OnPacket(IPacket packet)
        => Context.Pipelines.UserOnPacket.Process(new UserOnPacket<IShopStageUser>(this, packet));

    public override Task OnException(Exception exception)
        => Context.Pipelines.UserOnException.Process(new UserOnException<IShopStageUser>(this, exception));

    public override Task OnDisconnect()
        => Context.Pipelines.UserOnDisconnect.Process(new UserOnDisconnect<IShopStageUser>(this));
}
