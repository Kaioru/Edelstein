using System.Runtime.Serialization;
using Edelstein.Protocol.Services.Migration.Entities;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public class MigrationServiceClaimResponse
{
    [DataMember(Order = 1)] public required MigrationServiceResult Result { get; init; }
    [DataMember(Order = 2)] public MigrationServiceMigrationInfo? Info { get; init; }
}
