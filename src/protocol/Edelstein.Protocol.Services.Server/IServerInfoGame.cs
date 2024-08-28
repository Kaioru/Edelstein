namespace Edelstein.Protocol.Services.Server;

public interface IServerInfoGame : IServerInfo
{
    int WorldID { get; }
    int ChannelID { get; }
    bool IsAdultChannel { get; }
}
