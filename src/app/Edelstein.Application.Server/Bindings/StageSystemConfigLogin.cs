using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Application.Server.Bindings;

public record StageSystemConfigLogin : ILoginStageSystemOptions
{
    public required string ID { get; init; }
    
    public required string Host { get; init; }
    public required int Port { get; init; }
}
