using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerEntity : IServerEntry
{
    public required string ID { get; set; }
    
    public required string Host { get; set; }
    public required int Port { get; set; }
    
    public required long Token { get; set; }
    
    public required DateTime DateUpdated { get; set; }
    public required DateTime DateExpire { get; set; }
    
    public required ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
    public required ICollection<MigrationEntity> MigrationOut { get; set; } = new List<MigrationEntity>();
    public required ICollection<MigrationEntity> MigrationIn { get; set; } = new List<MigrationEntity>();
}
