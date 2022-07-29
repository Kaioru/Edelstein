using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Protocol.Services.Auth;

public interface IAuthService
{
    Task<IAuthResponse> Login(IAuthRequest request);
    Task<IAuthResponse> Register(IAuthRequest request);
}
