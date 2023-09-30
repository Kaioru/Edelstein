using Edelstein.Plugin.Rue.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCreateSecurityHandleCustomPlug : IPipelinePlug<UserOnPacketCreateSecurityHandle>
{
    private readonly ILoginManager _loginManager;

    public UserOnPacketCreateSecurityHandleCustomPlug(ILoginManager loginManager) => _loginManager = loginManager;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCreateSecurityHandle message)
    {
        (string, string) credentials = _loginManager.Options.AutoLoginCredentials;

        await _loginManager.Pipelines.UserOnPacketCheckPassword.Process(
            new UserOnPacketCheckPassword(message.User, credentials.Item1, credentials.Item2));
        ctx.Cancel();
    }
}
