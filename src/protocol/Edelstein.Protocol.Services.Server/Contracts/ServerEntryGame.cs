using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerEntryGame : ServerEntry, IServerEntryGame
{
    [DataMember(Order = 4)] public required int WorldID { get; init; }
    [DataMember(Order = 5)] public required int ChannelID { get; init; }
}
