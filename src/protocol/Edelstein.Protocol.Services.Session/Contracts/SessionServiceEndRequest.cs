using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceEndRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    
    [DataMember(Order = 2)] public required long Secret { get; init; }
}
