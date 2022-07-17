using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth.Contracts;

public record AuthLoginResponse(AuthLoginResult Result) : IAuthLoginResponse;
