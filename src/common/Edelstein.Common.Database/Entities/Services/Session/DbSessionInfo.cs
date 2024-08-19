using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Session.Entities;

namespace Edelstein.Common.Database.Entities.Services.Session;

public record DbSessionInfo : SessionServiceSessionInfo
{
    public long Secret { get; set; }
    
    public required DbServerInfo Server { get; set; }
}
