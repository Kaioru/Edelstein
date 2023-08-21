namespace Edelstein.Protocol.Services.Server.Contracts;

public enum ServerResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyRegistered = 0x3,
    FailedNotRegistered = 0x4,
    FailedNotFound = 0x5
}

