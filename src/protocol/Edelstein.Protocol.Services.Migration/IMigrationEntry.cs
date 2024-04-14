namespace Edelstein.Protocol.Services.Migration;

public interface IMigrationEntry
{
    int AccountID { get; }
    int CharacterID { get; }

    string FromServerID { get; }
    string ToServerID { get; }

    long Key { get; }
}
