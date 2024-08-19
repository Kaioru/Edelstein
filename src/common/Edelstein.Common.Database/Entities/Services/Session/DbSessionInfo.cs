using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Session.Entities;

namespace Edelstein.Common.Database.Entities.Services.Session;

public record DbSessionInfo : SessionServiceSessionInfo
{
    public long Secret { get; set; }
    
    public DbServerInfo Server { get; set; }
    public DbMigrationInfo? Migration { get; set; }
}
