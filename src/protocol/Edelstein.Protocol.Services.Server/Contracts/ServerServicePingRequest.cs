using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public class ServerServicePingRequest
{
    [DataMember(Order = 1)] public required string ID { get; init; }
    [DataMember(Order = 2)] public required long Secret { get; init; }
}
