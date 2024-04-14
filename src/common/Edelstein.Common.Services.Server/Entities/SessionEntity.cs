using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Services.Server.Entities;

public record SessionEntity : ISessionEntry
{
    public required ServerEntity Server { get; set; }
    public required string ServerID { get; set; }
    
    public required int ActiveAccount { get; set; }
    public required int? ActiveCharacter { get; set; }
    
    public required long Key { get; set; }
}
