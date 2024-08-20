using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchServiceResponse
{
    [DataMember(Order = 1)] public required DispatchServiceResult Result { get; init; }
}
