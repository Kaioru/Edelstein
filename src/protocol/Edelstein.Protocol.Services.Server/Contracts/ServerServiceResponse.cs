using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerServiceResponse
{
    [DataMember(Order = 1)] public required ServerServiceResult Result { get; init; }
}
