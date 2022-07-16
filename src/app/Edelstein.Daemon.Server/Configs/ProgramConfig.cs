namespace Edelstein.Daemon.Server.Configs;

public record ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; }
}
