using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth.Contracts;

public record AuthResponse(AuthResult Result) : IAuthResponse;
