using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Database.Entities.Services.Session;

public record DbSessionInfo : SessionInfo
{
    public long Secret { get; set; }
    
    public DbAccount Account { get; set; }
    public DbCharacter? Character { get; set; }
    public DbServerInfo Server { get; set; }
    public DbMigrationInfo? Migration { get; set; }
}
