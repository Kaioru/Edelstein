using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceStartRequest
{
    [DataMember(Order = 1)] public required SessionInfo Info { get; init; }
}
