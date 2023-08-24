namespace Edelstein.Protocol.Services.Server;

public interface IServerGame : IServer
{
    int WorldID { get; }
    int ChannelID { get; }
    bool IsAdultChannel { get; }
}
