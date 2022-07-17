namespace Edelstein.Protocol.Services.Auth.Contracts;

public enum AuthRegisterResult
{
    Unknown = 0x0,
    Success = 0x1,
    FailedUnknown = 0x2,
    FailedUsernameExists = 0x3
}
