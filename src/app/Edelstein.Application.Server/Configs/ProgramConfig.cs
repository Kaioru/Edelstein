namespace Edelstein.Application.Server.Configs;

public class ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; } = new List<ProgramConfigStageLogin>();
}
