using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Protocol.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> Register(AuthRequest request);
}
