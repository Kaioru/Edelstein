namespace Edelstein.Protocol.Services.Server;

public interface IServerEntry
{
    string ID { get; }
    string Host { get; }
    int Port { get; }
}
