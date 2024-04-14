using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerEntityGame : ServerEntity, IServerEntryGame
{
    public required int WorldID { get; set; }
    public required int ChannelID { get; set; }
}
