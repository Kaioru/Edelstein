namespace Edelstein.Common.Services.Server.Models;

public record SessionModel
{
    public int ActiveAccount { get; set; }
    public int? ActiveCharacter { get; set; }

    public string ServerID { get; set; }

    public ServerModel Server { get; set; }
}
