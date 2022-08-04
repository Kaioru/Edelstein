using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth.Contracts;

public record AuthRequest(string Username, string Password) : IAuthRequest;
