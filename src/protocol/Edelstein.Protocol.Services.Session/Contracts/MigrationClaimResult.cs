namespace Edelstein.Protocol.Services.Session.Contracts;

public enum MigrationClaimResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedNotStarted = 0x3
}
