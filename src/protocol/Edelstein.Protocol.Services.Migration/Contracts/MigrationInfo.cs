using System.Runtime.Serialization;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationInfo : IMigrationInfo
{
    [DataMember(Order = 1)] public required int AccountID { get; init; }
    [DataMember(Order = 2)]  public required int AccountWorldDataID { get; init; }
    [DataMember(Order = 3)] public required int CharacterID { get; init; }
    
    [DataMember(Order = 4)] public required string FromServerID { get; init; }
    [DataMember(Order = 5)] public required string ToServerID { get; init; }
    
    [DataMember(Order = 6)] public required MigrationInfoSnapshot<Account> AccountSnapshot { get; init; }
    [DataMember(Order = 7)] public required MigrationInfoSnapshot<AccountWorldData> AccountWorldDataSnapshot { get; init; }
    [DataMember(Order = 8)] public required MigrationInfoSnapshot<Character> CharacterSnapshot { get; init; }
}
