namespace Edelstein.Protocol.Services.Session;

public interface ISessionInfo
{
    string ServerID { get; }

    int ActiveAccount { get; }
    int? ActiveCharacter { get; }
}
