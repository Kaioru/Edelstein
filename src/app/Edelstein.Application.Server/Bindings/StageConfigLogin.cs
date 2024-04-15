using Edelstein.Protocol.Gameplay.Login;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Edelstein.Application.Server.Bindings;

public record StageConfigLogin : ILoginStageSystemOptions
{
    public string ID { get; init; }
    
    public string Host { get; init; }
    public int Port { get; init; }
}
