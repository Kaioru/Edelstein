using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Services.Server.Entities;

public record SessionEntity : ISession
{

    public ServerEntity Server { get; set; }
    public int ActiveAccount { get; set; }
    public int? ActiveCharacter { get; set; }

    public string ServerID { get; set; }
}
