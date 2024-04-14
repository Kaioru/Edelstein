using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts.Requests;

[DataContract]
public record MigrationClaimRequest
{
    [DataMember(Order = 1)] public required int CharacterID { get; init; }
    [DataMember(Order = 2)] public required string ServerID { get; init; }
    [DataMember(Order = 3)] public required long Key { get; init; }
}
