namespace Edelstein.Protocol.Services.Auth.Contracts;

public interface IAuthLoginRequest
{
    string Username { get; }
    string Password { get; }
}
