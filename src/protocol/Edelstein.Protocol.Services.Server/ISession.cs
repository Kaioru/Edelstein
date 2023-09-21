namespace Edelstein.Protocol.Services.Server;

public interface ISession
{
    string ServerID { get; }

    int ActiveAccount { get; }
    int? ActiveCharacter { get; }
}
