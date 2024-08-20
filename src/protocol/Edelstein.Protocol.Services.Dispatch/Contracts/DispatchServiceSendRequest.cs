using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchServiceSendRequest
{
    [DataMember(Order = 1)] public required DispatchInfo Info { get; init; }
}
