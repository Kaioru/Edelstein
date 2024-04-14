namespace Edelstein.Protocol.Services.Server;

public interface IServerEntryGame : IServerEntry
{
    int WorldID { get; }
    int ChannelID { get; }
}
