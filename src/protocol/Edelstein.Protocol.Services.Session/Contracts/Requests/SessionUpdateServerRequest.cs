using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Requests;

[DataContract]
public record SessionUpdateServerRequest
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    [DataMember(Order = 2)] public required string ServerID { get; init; }
}
