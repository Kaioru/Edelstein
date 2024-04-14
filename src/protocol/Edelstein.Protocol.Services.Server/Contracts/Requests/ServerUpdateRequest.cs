using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerUpdateRequest
{
    [DataMember(Order = 1)] public required string ServerID { get; init; }
    [DataMember(Order = 2)] public required long Token { get; init; }
}
