using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Auth.Contracts;

[DataContract]
public record AuthServiceRequest
{
    [DataMember(Order = 1)] public required string Username { get; init; }
    [DataMember(Order = 2)] public required string Password { get; init; }
}
