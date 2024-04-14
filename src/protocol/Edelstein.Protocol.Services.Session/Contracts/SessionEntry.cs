using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionEntry : ISessionEntry
{
    [DataMember] public required string ServerID { get; init; } = string.Empty;
    
    [DataMember] public required int ActiveAccount { get; init; }
    [DataMember] public required int? ActiveCharacter { get; init; }
}
