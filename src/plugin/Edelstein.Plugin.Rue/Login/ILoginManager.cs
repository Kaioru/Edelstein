using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Login;

public interface ILoginManager
{
    ILogger Logger { get; }
    ILoginManagerOptions Options { get; }
    IAuthService Auth { get; }
    IAccountRepository Repository { get; }
    ISessionService Session { get; }
    ILoginStage Stage { get; }
    LoginContextPipelines Pipelines { get; }
}
