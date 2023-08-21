namespace Edelstein.Protocol.Services.Server.Contracts;

public record ServerGetOneResponse<TServer>(
    ServerResult Result,
    TServer? Server = default
);
