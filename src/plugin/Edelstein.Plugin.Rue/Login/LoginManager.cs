using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Login;

public class LoginManager : ILoginManager
{
    public LoginManager(
        ILogger logger,
        ILoginManagerOptions options,
        IAuthService auth,
        IAccountRepository repository,
        ISessionService session,
        ILoginStage stage,
        LoginContextPipelines pipelines)
    {
        Logger = logger;
        Options = options;
        Auth = auth;
        Repository = repository;
        Session = session;
        Stage = stage;
        Pipelines = pipelines;
    }

    public ILogger Logger { get; }
    public ILoginManagerOptions Options { get; }
    public IAuthService Auth { get; }
    public IAccountRepository Repository { get; }
    public ISessionService Session { get; }
    public ILoginStage Stage { get; }
    public LoginContextPipelines Pipelines { get; }
}
