using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketLogoutWorldPlug : IPipelinePlug<UserOnPacketLogoutWorld>
{
    public Task Handle(IPipelineContext ctx, UserOnPacketLogoutWorld message) 
    {
        message.User.State = LoginState.SelectWorld;
        message.User.AccountWorld = null;
        message.User.SelectedWorldID = null;
        message.User.SelectedChannelID = null;

        return Task.CompletedTask;
    }
}
