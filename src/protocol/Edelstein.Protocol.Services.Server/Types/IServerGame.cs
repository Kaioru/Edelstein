namespace Edelstein.Protocol.Services.Server.Types;

public interface IServerGame : IServer
{
    int WorldID { get; }
    int ChannelID { get; }
}
