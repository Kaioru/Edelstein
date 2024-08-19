namespace Edelstein.Protocol.Services.Migration.Contracts;

public enum MigrationServiceResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyStarted = 0x3,
    FailedNotStarted = 0x4,
    FailedInvalidServer = 0x5
}
