using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Auth.Contracts;

[DataContract]
public record AuthServiceResponse
{
    [DataMember(Order = 1)] public required AuthServiceResult Result { get; init; }
}
