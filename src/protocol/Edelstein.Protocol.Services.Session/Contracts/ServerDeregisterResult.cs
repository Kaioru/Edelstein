namespace Edelstein.Protocol.Services.Session.Contracts;

public enum ServerDeregisterResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedNotRegistered = 0x3
}
