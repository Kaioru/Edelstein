using System;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Migration.Entities;

namespace Edelstein.Common.Database.Entities.Services.Migration;

public record DbMigrationInfo : MigrationServiceMigrationInfo
{
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    
    public long Secret { get; set; }
    
    public DbAccount Account { get; set; }
    public DbAccountWorldData AccountWorldData { get; set; }
    
    public DbServerInfo FromServer { get; set; }
    public DbServerInfo ToServer { get; set; }
}
