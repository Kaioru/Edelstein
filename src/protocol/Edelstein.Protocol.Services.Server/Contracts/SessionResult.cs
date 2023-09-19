namespace Edelstein.Protocol.Services.Server.Contracts;

public enum SessionResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyStarted = 0x3,
    FailedNotStarted = 0x4,
    FailedNotFound = 0x5
}
