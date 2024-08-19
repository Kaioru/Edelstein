using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts;

[DataContract]
public record SessionServiceStartResponse
{
    [DataMember(Order = 1)] public required SessionServiceResult Result { get; init; }
    [DataMember(Order = 2)] public long? Secret { get; init; }
}
