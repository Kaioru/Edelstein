using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerGetAllResponse<out TServer> where TServer : IServer
{
    ServerGetResult Result { get; }
    IEnumerable<TServer> Servers { get; }
}
