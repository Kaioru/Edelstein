namespace Edelstein.Protocol.Services.Auth.Contracts;

public enum AuthLoginResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedInvalidUsername = 0x3,
    FailedInvalidPassword = 0x4
}
