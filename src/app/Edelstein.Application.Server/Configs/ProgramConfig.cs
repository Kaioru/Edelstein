namespace Edelstein.Application.Server.Configs;

public class ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; } = new List<ProgramConfigStageLogin>();
    public ICollection<ProgramConfigStageGame> GameStages { get; set; } = new List<ProgramConfigStageGame>();
    public ICollection<ProgramConfigStageShop> ShopStages { get; set; } = new List<ProgramConfigStageShop>();
    public ICollection<ProgramConfigStageTrade> TradeStages { get; set; } = new List<ProgramConfigStageTrade>();

    public bool MigrateDatabaseOnInit { get; set; } = false;
    public int TicksPerSecond { get; set; } = 4;
    
    public ICollection<string> Plugins { get; set; } = new List<string>();
}
