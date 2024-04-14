using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Session.Contracts.Responses;

[DataContract]
public record SessionGetOneResponse
{
    [DataMember(Order = 1)] public required SessionResult Result { get; init; }
    [DataMember(Order = 2)] public required SessionEntry? Session { get; init; }
}
