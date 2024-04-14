using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerRegisterRequest<TServerEntry>
    where TServerEntry : ServerEntry
{
    [DataMember(Order = 1)] public required TServerEntry Server { get; init; }
}
