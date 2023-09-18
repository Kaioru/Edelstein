using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Trade;

public interface ITradeStageOptions : IIdentifiable<string>
{
    int RegisterFeeMeso { get; }
    int CommissionRate { get; }
    int CommissionBase { get; }
    int AuctionDurationMin { get; }
    int AuctionDurationMax { get; }
}
