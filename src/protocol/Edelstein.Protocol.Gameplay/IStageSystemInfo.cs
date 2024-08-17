namespace Edelstein.Protocol.Gameplay;

public interface IStageSystemInfo
{
    string ID { get; }
    
    string Host { get; }
    int Port { get; }
}
