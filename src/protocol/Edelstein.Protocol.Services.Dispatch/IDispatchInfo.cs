namespace Edelstein.Protocol.Services.Dispatch;

public interface IDispatchInfo
{
    public string? TargetServerID { get; init; }
    public int? TargetWorldID { get; init; }
    public int? TargetChannelID { get; init; }
    public int? TargetCharacterID { get; init; }
    
    byte[] Payload { get; }
}
