using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerGetGameByWorldAndChannelRequest
{
    [DataMember(Order = 1)] public required string WorldID { get; init; }
    [DataMember(Order = 2)] public required string ChannelID { get; init; }
}
