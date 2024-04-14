using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Requests;

[DataContract]
public record SessionStartRequest
{
    [DataMember(Order = 1)] public required SessionEntry Session { get; init; }
    [DataMember(Order = 2)] public required long Key { get; init; }
}
