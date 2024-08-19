using System.Runtime.Serialization;
using Edelstein.Protocol.Services.Migration.Entities;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationServiceClaimRequest
{
    [DataMember(Order = 1)] public required int CharacterID { get; init; }
    [DataMember(Order = 2)] public required string ServerID { get; init; }
    
    [DataMember(Order = 3)] public required long Secret { get; init; }
}
