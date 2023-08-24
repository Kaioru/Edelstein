namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerGetAllResponse<TServer>(
    ServerResult Result,
    IEnumerable<TServer> Servers
);
