namespace Edelstein.Protocol.Services.Session.Contracts;

public record SessionGetOneResponse(
    SessionResult Result,
    ISession? Session = null
);
