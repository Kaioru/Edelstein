namespace Edelstein.Daemon.Server.Configs;

#pragma warning disable CS8618

public record ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; }
}
