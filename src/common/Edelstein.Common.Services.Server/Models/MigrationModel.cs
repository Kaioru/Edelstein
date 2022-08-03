namespace Edelstein.Common.Services.Server.Models;

public record MigrationModel
{
    public string FromServerID { get; set; }
    public string ToServerID { get; set; }

    public ServerModel FromServer { get; set; }
    public ServerModel ToServer { get; set; }

    public long Key { get; set; }

    public byte[] AccountBytes { get; set; }
    public byte[] AccountWorldBytes { get; set; }
    public byte[] CharacterBytes { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }

    public int AccountID { get; set; }
    public int CharacterID { get; set; }
}
