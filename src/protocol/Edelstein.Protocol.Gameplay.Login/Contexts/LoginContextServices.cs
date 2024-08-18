using Edelstein.Protocol.Services.Auth;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextServices(
    IAuthService Auth
);
