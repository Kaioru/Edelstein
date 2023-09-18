using Edelstein.Protocol.Gameplay.Trade.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Trade;

public class TradeStageUserInitializer : IAdapterInitializer
{
    private readonly TradeContext _context;

    public TradeStageUserInitializer(TradeContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new TradeStageUser(socket, _context);
}
