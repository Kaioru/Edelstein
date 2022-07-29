using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Protocol.Services.Server.Contracts;

public interface IServerGetOneResponse<out TServer> : IServerResponse where TServer : IServer
{
    TServer? Server { get; }
}
