using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Application.Server.Bindings;

public record GameStageSystemConfig : IGameStageSystemOptions
{
    public required string ID { get; init; }
    
    public required string Host { get; init; }
    public required int Port { get; init; }
    
    public required int WorldID { get; init; }
    public required int ChannelID { get; init; }
    public required bool IsAdultChannel { get; init; }
}
