using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Server;

public interface IServer: IIdentifiable<string>
{
    string Host { get; }
    int Port { get; }
}
