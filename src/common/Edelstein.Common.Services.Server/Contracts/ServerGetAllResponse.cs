using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Common.Services.Server.Contracts;

public record ServerGetAllResponse<TServer>(
    ServerResult Result,
    IEnumerable<TServer> Servers
) : IServerGetAllResponse<TServer> where TServer : IServer;
