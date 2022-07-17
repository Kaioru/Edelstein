using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth.Contracts;

public record AuthLoginRequest(string Username, string Password) : IAuthLoginRequest;
