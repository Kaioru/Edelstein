using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerGameEntity : ServerEntity, IServerGame
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }
    public bool IsAdultChannel { get; set; }
}
