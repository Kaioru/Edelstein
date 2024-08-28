using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Database.Entities.Services.Server;

public record DbServerInfoGame : DbServerInfo, IServerInfoGame
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }
    public bool IsAdultChannel { get; set; }
}
