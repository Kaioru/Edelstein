using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerTradeEntity : ServerEntity, IServerTrade
{
    public int WorldID { get; set; }
}
