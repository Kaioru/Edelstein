using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Responses;

[DataContract]
public record ServerGetAllResponse<TServerEntry>
    where TServerEntry : ServerEntry
{
    [DataMember(Order = 1)] public required ServerResult Result { get; init; }
    [DataMember(Order = 2)] public required IEnumerable<TServerEntry> Servers { get; init; }
}
