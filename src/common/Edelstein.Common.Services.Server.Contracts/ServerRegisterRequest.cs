using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerRegisterRequest<TServer>(TServer Server) : IServerRegisterRequest<TServer> where TServer : IServer;
