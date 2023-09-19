using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerShopEntity : ServerEntity, IServerShop
{
    public int WorldID { get; set; }
}
