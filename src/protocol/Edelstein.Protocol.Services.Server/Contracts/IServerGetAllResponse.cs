using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Protocol.Services.Server.Contracts;

public interface IServerGetAllResponse<out TServer> : IServerResponse where TServer : IServer
{
    IEnumerable<TServer> Servers { get; }
}
