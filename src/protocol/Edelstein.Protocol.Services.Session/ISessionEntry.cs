namespace Edelstein.Protocol.Services.Session;

public interface ISessionEntry
{
    string ServerID { get; }
    
    int ActiveAccount { get; }
    int? ActiveCharacter { get; }
}
