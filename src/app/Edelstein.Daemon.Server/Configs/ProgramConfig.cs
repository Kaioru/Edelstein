namespace Edelstein.Daemon.Server.Configs;

#pragma warning disable CS8618

public record ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; } = new List<ProgramConfigStageLogin>();
    public ICollection<ProgramConfigStageGame> GameStages { get; set; } = new List<ProgramConfigStageGame>();

    public int TicksPerSecond { get; set; } = 4;

    public ICollection<string> Plugins { get; set; } = new List<string>();
}
