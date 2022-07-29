using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerGetAllResponse<out TServer> : IServerResponse where TServer : IServer
{
    ServerResult Result { get; }
    IEnumerable<TServer> Servers { get; }
}
