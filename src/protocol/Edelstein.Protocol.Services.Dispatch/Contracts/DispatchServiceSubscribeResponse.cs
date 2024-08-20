using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchServiceSubscribeResponse
{
    [DataMember(Order = 1)] public required DispatchInfo Info { get; init; }
}
