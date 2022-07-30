using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Server.Models;

public record ServerModel : IIdentifiable<string>
{
    public string Host { get; set; }
    public int Port { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public ICollection<SessionModel> Sessions { get; set; } = new List<SessionModel>();

    public ICollection<MigrationModel> MigrationOut { get; set; } = new List<MigrationModel>();
    public ICollection<MigrationModel> MigrationIn { get; set; } = new List<MigrationModel>();

    public string ID { get; set; }
}
