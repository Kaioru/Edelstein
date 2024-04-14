using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerEntityShop : ServerEntity, IServerEntryShop
{
    public required int WorldID { get; set; }
}
