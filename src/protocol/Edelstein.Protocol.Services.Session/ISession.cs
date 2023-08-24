namespace Edelstein.Protocol.Services.Session;

public interface ISession
{
    string ServerID { get; }

    int ActiveAccount { get; }
    int? ActiveCharacter { get; }
}
