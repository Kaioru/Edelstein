using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerServiceGetByWorldAndChannelRequest
{
    [DataMember(Order = 1)] public required int WorldID { get; init; }
    [DataMember(Order = 2)] public required int ChannelID { get; init; }
}

