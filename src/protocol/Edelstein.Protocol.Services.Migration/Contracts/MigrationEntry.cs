using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationEntry : IMigrationEntry
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    [DataMember(Order = 2)] public required int CharacterID { get; init; }
    
    [DataMember(Order = 3)] public required string FromServerID { get; init; }
    [DataMember(Order = 4)] public required string ToServerID { get; init; }
    
    [DataMember(Order = 5)] public required long Key { get; init; }
}
