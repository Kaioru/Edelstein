using Edelstein.Protocol.Services.Auth;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextServices
{
    IAuthService Auth { get; }
}
