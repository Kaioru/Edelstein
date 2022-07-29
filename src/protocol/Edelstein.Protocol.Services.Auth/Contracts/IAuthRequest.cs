namespace Edelstein.Protocol.Services.Auth.Contracts;

public interface IAuthRequest
{
    string Username { get; }
    string Password { get; }
}
