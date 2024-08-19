using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Server;

public interface IServerInfo : IRepositoryEntry<string>
{
    string Host { get; }
    int Port { get; }
}
