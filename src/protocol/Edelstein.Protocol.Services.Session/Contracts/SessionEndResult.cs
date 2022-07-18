namespace Edelstein.Protocol.Services.Session.Contracts;

public enum SessionEndResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedNotActive = 0x3
}
