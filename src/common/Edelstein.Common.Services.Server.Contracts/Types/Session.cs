using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Common.Services.Server.Contracts.Types;

public record Session : ISession
{
    public string ServerID { get; set; }

    public int ActiveAccount { get; set; }
    public int? ActiveCharacter { get; set; }
}
