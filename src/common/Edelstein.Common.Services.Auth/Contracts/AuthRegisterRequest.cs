using Edelstein.Protocol.Services.Auth.Contracts;

namespace Edelstein.Common.Services.Auth.Contracts;

public record AuthRegisterRequest(string Username, string Password) : IAuthRegisterRequest;
