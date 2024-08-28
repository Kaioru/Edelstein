using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerInfoGame : IServerInfoGame
{
    [DataMember(Order = 1)] public required string ID { get; init; }
    [DataMember(Order = 2)] public required string Host { get; init; }
    [DataMember(Order = 3)] public required int Port { get; init; }
    
    [DataMember(Order = 4)] public required int WorldID { get; init; }
    [DataMember(Order = 5)] public required int ChannelID { get; init; }
    [DataMember(Order = 6)] public bool IsAdultChannel { get; init; }
}
