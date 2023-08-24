namespace Edelstein.Protocol.Services.Session.Contracts;

public record Session(
    string ServerID,
    int ActiveAccount,
    int? ActiveCharacter = null
) : ISession;
