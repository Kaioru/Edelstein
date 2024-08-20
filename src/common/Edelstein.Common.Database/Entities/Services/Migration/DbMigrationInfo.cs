using System;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Database.Entities.Services.Migration;

public record DbMigrationInfo : MigrationInfo
{
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    
    public DbAccount Account { get; set; }
    public DbAccountWorldData AccountWorldData { get; set; }
    
    public DbSessionInfo Session { get; set; }
    
    public DbServerInfo FromServer { get; set; }
    public DbServerInfo ToServer { get; set; }
}
