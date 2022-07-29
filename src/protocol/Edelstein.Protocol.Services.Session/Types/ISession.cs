namespace Edelstein.Protocol.Services.Session.Types;

public interface ISession
{
    string ServerID { get; }

    int ActiveAccount { get; }
    int? ActiveCharacter { get; }
}
