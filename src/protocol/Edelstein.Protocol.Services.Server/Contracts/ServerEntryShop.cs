using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerEntryShop : ServerEntry, IServerEntryShop
{
    [DataMember(Order = 4)] public required int WorldID { get; init; }
}
