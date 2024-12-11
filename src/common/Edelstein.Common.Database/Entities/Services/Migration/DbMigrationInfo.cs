using System;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Database.Entities.Services.Migration;

public record DbMigrationInfo : MigrationInfo
{
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    
    public required DbAccount Account { get; set; }
    public required DbAccountWorldData AccountWorldData { get; set; }
    public required DbCharacter Character { get; set; }
    
    public required DbSessionInfo Session { get; set; }
    
    public required DbServerInfo FromServer { get; set; }
    public required DbServerInfo ToServer { get; set; }
}
