using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Protocol.Services.Auth;

public interface IAuthService
{
    Task<IAuthLoginResponse> Login(IAuthLoginRequest request);
    Task<IAuthRegisterResponse> Register(IAuthRegisterRequest request);
}
