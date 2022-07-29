using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Server.Types;

public interface IServer : IIdentifiable<string>
{
    string Host { get; }
    int Port { get; }
}
