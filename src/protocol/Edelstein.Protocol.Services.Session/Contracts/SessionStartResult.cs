namespace Edelstein.Protocol.Services.Session.Contracts;

public enum SessionStartResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyStarted = 0x3
}
