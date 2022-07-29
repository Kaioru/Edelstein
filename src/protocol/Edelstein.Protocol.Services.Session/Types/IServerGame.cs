namespace Edelstein.Protocol.Services.Session.Types;

public interface IServerGame : IServer
{
    int WorldID { get; }
    int ChannelID { get; }
}
