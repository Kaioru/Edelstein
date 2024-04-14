using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerGetGameByWorldRequest
{
    [DataMember(Order = 1)] public required int WorldID { get; init; }
}
