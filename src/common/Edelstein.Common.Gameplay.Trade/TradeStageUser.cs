using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Gameplay.Trade.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Trade;

public class TradeStageUser : AbstractStageUser<ITradeStageUser>, ITradeStageUser
{
    public TradeStageUser(ISocket socket, TradeContext context) : base(socket) 
        => Context = context;

    public TradeContext Context { get; }
    public string? FromServerID { get; set; }

    public override Task Migrate(string serverID, IPacket? packet = null)
        => Context.Pipelines.UserMigrate.Process(new UserMigrate<ITradeStageUser>(this, serverID, packet));

    public override Task OnPacket(IPacket packet)
        => Context.Pipelines.UserOnPacket.Process(new UserOnPacket<ITradeStageUser>(this, packet));

    public override Task OnException(Exception exception)
        => Context.Pipelines.UserOnException.Process(new UserOnException<ITradeStageUser>(this, exception));

    public override Task OnDisconnect()
        => Context.Pipelines.UserOnDisconnect.Process(new UserOnDisconnect<ITradeStageUser>(this));
}
