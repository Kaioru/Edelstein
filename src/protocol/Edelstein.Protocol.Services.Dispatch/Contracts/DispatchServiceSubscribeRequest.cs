using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchServiceSubscribeRequest
{
    [DataMember(Order = 1)]
    public required string ServerID { get; init; }
    
    [DataMember(Order = 2)]
    public int? WorldID { get; init; }
    
    [DataMember(Order = 3)]
    public int? ChannelID { get; init; }
}
