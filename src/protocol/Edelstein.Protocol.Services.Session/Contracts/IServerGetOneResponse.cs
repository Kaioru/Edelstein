using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerGetOneResponse<out TServer> : IServerResponse where TServer : IServer
{
    TServer? Server { get; }
}
