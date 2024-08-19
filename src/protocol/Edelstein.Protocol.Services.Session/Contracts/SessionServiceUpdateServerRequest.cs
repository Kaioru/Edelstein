using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceUpdateServerRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    [DataMember(Order = 2)] public required string ServerID { get; init; }
    
    [DataMember(Order = 3)] public required long Secret { get; init; }
}
