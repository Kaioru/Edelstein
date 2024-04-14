using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts.Requests;

[DataContract]
public record MigrationStartRequest
{
    [DataMember(Order = 1)] public required MigrationEntry Migration { get; init; }
}
