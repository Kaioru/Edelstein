namespace Edelstein.Protocol.Services.Dispatch;

public interface IDispatchInfo
{
    DispatchTarget TargetType { get; }
    int TargetID { get; }
    
    byte[] Payload { get; }
}
