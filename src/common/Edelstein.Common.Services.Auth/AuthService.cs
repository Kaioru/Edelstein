using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth;

public class AuthService : IAuthService
{
    public Task<IAuthLoginResponse> Login(IAuthLoginRequest request) =>
        Task.FromResult<IAuthLoginResponse>(new AuthLoginResponse(AuthLoginResult.Success));

    public Task<IAuthRegisterResponse> Register(IAuthRegisterRequest request) =>
        Task.FromResult<IAuthRegisterResponse>(new AuthRegisterResponse(AuthRegisterResult.Success));
}
