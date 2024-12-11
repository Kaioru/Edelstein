using System;
using System.Collections.Generic;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Common.Database.Entities.Services.Server;

public record DbServerInfo : ServerInfo
{
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    
    public long Secret { get; set; }

    public required ICollection<DbSessionInfo> Sessions { get; set; }
    
    public required ICollection<DbMigrationInfo> MigrationOut { get; set; }
    public required ICollection<DbMigrationInfo> MigrationIn { get; set; }
}
