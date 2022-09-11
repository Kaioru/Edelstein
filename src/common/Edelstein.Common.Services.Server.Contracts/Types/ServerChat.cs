using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Contracts.Types;

public record ServerChat : Server, IServerChat
{
    public int WorldID { get; set; }
}
