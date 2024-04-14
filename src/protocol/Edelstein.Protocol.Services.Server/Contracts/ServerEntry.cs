using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerEntry : IServerEntry
{
    [DataMember(Order = 1)] public required string ID { get; init; }
    [DataMember(Order = 2)] public required string Host { get; init; }
    [DataMember(Order = 3)] public required int Port { get; init; }
}
