using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerEntryTrade : ServerEntry, IServerEntryTrade
{
    [DataMember(Order = 4)] public required int WorldID { get; init; }
}
