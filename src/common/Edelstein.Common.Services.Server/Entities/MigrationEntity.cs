using System;
using Edelstein.Protocol.Services.Migration;

namespace Edelstein.Common.Services.Server.Entities;

public record MigrationEntity : IMigrationEntry
{
    public required int AccountID { get; set; }
    public required int CharacterID { get; set; }
    
    public required ServerEntity FromServer { get; set; }
    public required string FromServerID { get; set; }
    
    public required ServerEntity ToServer { get; set; }
    public required string ToServerID { get; set; }
    
    public required long Key { get; set; }
    
    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
}
