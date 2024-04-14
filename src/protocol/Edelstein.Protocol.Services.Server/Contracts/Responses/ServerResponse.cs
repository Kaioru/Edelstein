using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Responses;

[DataContract]
public record ServerResponse
{
    [DataMember(Order = 1)] public required ServerResult Result { get; init; }
}
