using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Server.Entities;

public record ServerEntity : IIdentifiable<string>, IServer
{
    public string ID { get; set; }
    
    public string Host { get; set; }
    public int Port { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();

    public ICollection<MigrationEntity> MigrationOut { get; set; } = new List<MigrationEntity>();
    public ICollection<MigrationEntity> MigrationIn { get; set; } = new List<MigrationEntity>();
}
