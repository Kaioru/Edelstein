namespace Edelstein.Protocol.Services.Server.Contracts;

public interface IServerGetGameByWorldAndChannelRequest
{
    int WorldID { get; }
    int ChannelID { get; }
}
