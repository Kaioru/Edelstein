using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerGetOneResponse<out TServer> where TServer : IServer
{
    ServerGetResult Result { get; }
    TServer? Server { get; }
}
