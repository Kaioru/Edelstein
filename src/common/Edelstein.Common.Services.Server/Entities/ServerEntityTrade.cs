using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerEntityTrade : ServerEntity, IServerEntryTrade
{
    public required int WorldID { get; set; }
}
