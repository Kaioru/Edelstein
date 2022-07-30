using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerGetGameByWorldAndChannelRequest(
    int WorldID,
    int ChannelID
) : IServerGetGameByWorldAndChannelRequest;
