using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts.Responses;

[DataContract]
public record MigrationResponse
{
    [DataMember(Order = 1)] public required MigrationResult Result { get; init; }
}
