namespace Edelstein.Protocol.Services.Server.Contracts;

public interface IServerGetGameByWorldAndChannel
{
    int WorldID { get; }
    int ChannelID { get; }
}
