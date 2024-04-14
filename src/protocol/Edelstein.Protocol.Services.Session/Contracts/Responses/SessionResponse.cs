using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Responses;

[DataContract]
public record SessionResponse
{
    [DataMember(Order = 1)] public required SessionResult Result { get; init; }
}
