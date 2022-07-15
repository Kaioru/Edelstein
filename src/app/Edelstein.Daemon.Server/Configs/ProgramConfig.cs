namespace Edelstein.Daemon.Server.Configs;

public record ProgramConfig
{
    public ICollection<ProgramConfigStage> Stages { get; set; }
}
