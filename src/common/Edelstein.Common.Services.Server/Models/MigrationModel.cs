using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Server.Models;

public record MigrationModel : IIdentifiable<int>
{
    public string FromServerID { get; set; }
    public string ToServerID { get; set; }

    public ServerModel FromServer { get; set; }
    public ServerModel ToServer { get; set; }

    public string Key { get; set; }

    public byte[] AccountBytes { get; set; }
    public byte[] AccountWorldBytes { get; set; }
    public byte[] CharacterBytes { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public int ID { get; set; }
}
