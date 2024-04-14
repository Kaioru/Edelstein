using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerGetGameByWorldAndChannelRequest
{
    [DataMember(Order = 1)] public required int WorldID { get; init; }
    [DataMember(Order = 2)] public required int ChannelID { get; init; }
}
