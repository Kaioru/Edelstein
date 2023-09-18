using Edelstein.Protocol.Gameplay.Trade.Contexts;

namespace Edelstein.Protocol.Gameplay.Trade;

public interface ITradeStageUser : IStageUser<ITradeStageUser>
{
    TradeContext Context { get; }
    
    string? FromServerID { get; set; }
}
