using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationServiceStartRequest
{
    [DataMember(Order = 1)] public required MigrationInfo Info { get; init; }
}
