using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Dispatch.Contracts;

[DataContract]
public record DispatchInfo : IDispatchInfo
{
    [DataMember(Order = 1)] public string? TargetServerID { get; init; }
    [DataMember(Order = 2)] public int? TargetWorldID { get; init; }
    [DataMember(Order = 3)] public int? TargetChannelID { get; init; }
    [DataMember(Order = 4)] public int? TargetCharacterID { get; init; }
    
    [DataMember(Order = 5)] public required byte[] Payload { get; init; }
}
