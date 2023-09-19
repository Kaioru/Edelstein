namespace Edelstein.Protocol.Services.Server.Contracts;

public record SessionGetOneResponse(
    SessionResult Result,
    ISession? Session = null
);
