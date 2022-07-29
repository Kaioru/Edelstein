namespace Edelstein.Protocol.Services.Session.Contracts;

public enum ServerRegisterResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedAlreadyRegistered = 0x3
}
