namespace Edelstein.Protocol.Services.Auth.Contracts;

public interface IAuthRegisterRequest
{
    string Username { get; }
    string Password { get; }
}
