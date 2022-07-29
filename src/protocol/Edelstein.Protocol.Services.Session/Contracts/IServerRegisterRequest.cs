using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IServerRegisterRequest<out TServer> where TServer : IServer
{
    TServer Server { get; }
}
