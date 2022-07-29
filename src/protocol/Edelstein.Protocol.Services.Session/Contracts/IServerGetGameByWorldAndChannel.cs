namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerGetGameByWorldAndChannel
{
    int WorldID { get; }
    int ChannelID { get; }
}
