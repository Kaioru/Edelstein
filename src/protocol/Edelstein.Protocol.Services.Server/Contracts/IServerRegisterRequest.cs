using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Protocol.Services.Server.Contracts;

public interface IServerRegisterRequest<out TServer> where TServer : IServer
{
    TServer Server { get; }
}
