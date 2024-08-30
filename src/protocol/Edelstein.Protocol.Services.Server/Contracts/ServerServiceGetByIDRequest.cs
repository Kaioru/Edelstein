using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerServiceGetByIDRequest
{
    [DataMember(Order = 1)] public required string ID { get; init; }
}
