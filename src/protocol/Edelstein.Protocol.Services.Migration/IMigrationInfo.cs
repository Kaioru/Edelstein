namespace Edelstein.Protocol.Services.Migration;

public interface IMigrationInfo
{
    int AccountID { get; }
    int AccountWorldDataID { get; }
    int CharacterID { get; }

    string FromServerID { get; }
    string ToServerID { get; }
}
