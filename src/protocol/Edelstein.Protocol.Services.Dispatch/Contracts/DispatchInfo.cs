using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchInfo : IDispatchInfo
{
    [DataMember(Order = 1)]
    public required DispatchTarget TargetType { get; init; }
    
    [DataMember(Order = 2)]
    public required int TargetID { get; init; }
    
    [DataMember(Order = 3)]
    public required byte[] Payload { get; init; }
}
