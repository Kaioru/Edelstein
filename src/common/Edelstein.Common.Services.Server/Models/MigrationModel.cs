using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Server.Models;

public record MigrationModel : IIdentifiable<int>
{
    public ServerModel FromServer { get; set; }
    public ServerModel ToServer { get; set; }

    public string Key { get; set; }

    public int ID { get; set; }
}
