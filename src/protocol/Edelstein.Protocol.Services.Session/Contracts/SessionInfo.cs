using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionInfo : ISessionInfo
{
    [DataMember(Order = 1)] public required string ServerID { get; init; }
    
    [DataMember(Order = 2)] public required int ActiveAccount { get; init; }
    [DataMember(Order = 3)] public int? ActiveCharacter { get; init; }
}
