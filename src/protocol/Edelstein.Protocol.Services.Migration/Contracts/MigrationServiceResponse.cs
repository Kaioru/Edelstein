using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationServiceResponse
{
    [DataMember(Order = 1)] public required MigrationServiceResult Result { get; init; }
}
