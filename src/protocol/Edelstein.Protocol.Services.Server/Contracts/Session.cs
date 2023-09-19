namespace Edelstein.Protocol.Services.Server.Contracts;

public record Session(
    string ServerID,
    int ActiveAccount,
    int? ActiveCharacter = null
) : ISession;
