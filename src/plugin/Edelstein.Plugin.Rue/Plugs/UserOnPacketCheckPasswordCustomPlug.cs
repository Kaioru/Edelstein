using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Plugin.Rue.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordCustomPlug : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILoginManager _loginManager;

    public UserOnPacketCheckPasswordCustomPlug(ILoginManager loginManager) => _loginManager = loginManager;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        LoginResult result;

        switch (_loginManager.Options.SkipAuthorization)
        {
            case true:
            {
                _loginManager.Logger.LogInformation($"Skipping authorization for user {message.Username} with password {message.Password}");
                result = LoginResult.Success;
                break;
            }

            case false:
            {
                var response = await _loginManager.Auth.Login(new AuthRequest(message.Username, message.Password));

                result = response.Result switch
                {
                    AuthResult.Success => LoginResult.Success,
                    AuthResult.FailedInvalidUsername => LoginResult.NotRegistered,
                    AuthResult.FailedInvalidPassword => LoginResult.IncorrectPassword,
                    _ => LoginResult.Unknown
                };
                break;
            }
        }

        if (_loginManager.Options.IsAutoRegister && result == LoginResult.NotRegistered)
        {
            result = LoginResult.Success;
            await _loginManager.Auth.Register(new AuthRequest(message.Username, message.Password));
            _loginManager.Logger.LogInformation($"Created new user {message.Username} with password {message.Password}");
        }
    }
}
