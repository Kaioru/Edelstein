using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Services.Auth;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextServices(
    IAuthService Auth
) : ILoginContextServices;
