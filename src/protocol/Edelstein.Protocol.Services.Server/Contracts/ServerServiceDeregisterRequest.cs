using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServiceDeregisterRequest
{
    [DataMember(Order = 1)] public required int ID { get; init; }
    [DataMember(Order = 2)] public required int Secret { get; init; }
}
