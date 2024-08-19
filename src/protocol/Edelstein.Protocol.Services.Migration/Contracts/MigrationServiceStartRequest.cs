using System.Runtime.Serialization;
using Edelstein.Protocol.Services.Migration.Entities;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationServiceStartRequest
{
    [DataMember(Order = 1)] public required MigrationServiceMigrationInfo Info { get; init; }
}
