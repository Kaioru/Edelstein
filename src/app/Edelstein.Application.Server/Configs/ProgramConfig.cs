namespace Edelstein.Application.Server.Configs;

public class ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; } = new List<ProgramConfigStageLogin>();
    public ICollection<ProgramConfigStageGame> GameStages { get; set; } = new List<ProgramConfigStageGame>();

    public bool MigrateDatabaseOnInit { get; set; } = false;
    public int TicksPerSecond { get; set; } = 4;
    
    public ICollection<string> Plugins { get; set; } = new List<string>();
}
