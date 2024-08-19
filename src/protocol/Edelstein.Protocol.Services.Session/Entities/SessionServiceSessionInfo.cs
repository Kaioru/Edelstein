using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Entities;

[DataContract]
public record SessionServiceSessionInfo : ISessionInfo
{
    [DataMember(Order = 1)] public required string ServerID { get; init; }
    
    [DataMember(Order = 2)] public required int ActiveAccount { get; init; }
    [DataMember(Order = 3)] public int? ActiveCharacter { get; init; }
}
