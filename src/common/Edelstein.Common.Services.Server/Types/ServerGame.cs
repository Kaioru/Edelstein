using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Types;

public record ServerGame : Server, IServerGame
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }
    public bool IsAdultChannel { get; set; }
}
