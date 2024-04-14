using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Responses;

[DataContract]
public record ServerRegisterResponse
{
    [DataMember(Order = 1)] public required ServerResult Result { get; init; }
    [DataMember(Order = 2)] public required long Token { get; init; }
}
