namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerGetGameByWorldAndChannelRequest(
    int WorldID,
    int ChannelID
);
