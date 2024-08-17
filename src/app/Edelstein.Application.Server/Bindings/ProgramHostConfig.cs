namespace Edelstein.Application.Server.Bindings;

public record ProgramHostConfig
{
    public required string StageDirectory { get; init; }
    public required string PluginDirectory { get; init; }
}
