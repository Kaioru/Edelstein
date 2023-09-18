using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStageTrade : ProgramConfigStage, ITradeStageOptions
{
    public int WorldID { get; set; }
    
    public int RegisterFeeMeso { get; set; }
    public int CommissionRate { get; set; }
    public int CommissionBase { get; set; }
    public int AuctionDurationMin { get; set; }
    public int AuctionDurationMax { get; set; }
}
