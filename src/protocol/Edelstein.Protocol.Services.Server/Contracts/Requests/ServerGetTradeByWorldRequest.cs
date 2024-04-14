using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts.Requests;

[DataContract]
public record ServerGetTradeByWorldRequest
{
    [DataMember(Order = 1)] public required int WorldID { get; init; }
}
