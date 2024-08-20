namespace Edelstein.Protocol.Services.Migration.Contracts;

public record MigrationInfo : IMigrationInfo
{
    public required int AccountID { get; init; }
    public required int AccountWorldDataID { get; init; }
    public required int CharacterID { get; init; }
    
    public required string FromServerID { get; init; }
    public required string ToServerID { get; init; }
}
