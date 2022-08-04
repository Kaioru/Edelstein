using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerGetOneResponse<TServer>(
    ServerResult Result,
    TServer? Server = default
) : IServerGetOneResponse<TServer> where TServer : IServer;
