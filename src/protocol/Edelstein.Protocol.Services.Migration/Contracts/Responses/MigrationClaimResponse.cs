using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts.Responses;

[DataContract]
public record MigrationClaimResponse
{
    [DataMember(Order = 1)] public required MigrationResult Result { get; init; }
    [DataMember(Order = 2)] public required MigrationEntry? Migration { get; init; }
}
