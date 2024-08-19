using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Entities;

[DataContract]
public record ServerServiceServerInfoLogin : IServerInfoLogin
{
    [DataMember(Order = 1)] public required string ID { get; init; }
    
    [DataMember(Order = 2)] public required string Host { get; init; }
    [DataMember(Order = 3)] public required int Port { get; init; }
}
