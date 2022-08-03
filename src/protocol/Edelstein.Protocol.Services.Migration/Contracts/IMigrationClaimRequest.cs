namespace Edelstein.Protocol.Services.Migration.Contracts;

public interface IMigrationClaimRequest
{
    int CharacterID { get; }
    string ServerID { get; }
    long Key { get; }
}
