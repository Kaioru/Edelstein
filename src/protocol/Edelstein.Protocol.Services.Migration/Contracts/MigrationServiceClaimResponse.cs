using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public class MigrationServiceClaimResponse
{
    [DataMember(Order = 1)] public required MigrationServiceResult Result { get; init; }
    [DataMember(Order = 2)] public MigrationInfo? Info { get; init; }
}
