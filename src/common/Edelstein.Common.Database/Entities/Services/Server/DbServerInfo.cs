using System;
using System.Collections.Generic;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Database.Entities.Services.Server;

public record DbServerInfo : IServerInfo
{
    public required string ID { get; set; }
    
    public required string Host { get; set; }
    public required int Port { get; set; }
    
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    
    public long Secret { get; set; }

    public ICollection<DbSessionInfo> Sessions { get; set; } = new List<DbSessionInfo>();
}
