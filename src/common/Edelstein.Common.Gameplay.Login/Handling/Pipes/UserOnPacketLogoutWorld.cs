using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketLogoutWorld : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, LogoutWorld>>
{
    public Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, LogoutWorld> message)
    {
        if (message.User.State != LoginState.SelectCharacter) return Task.CompletedTask;
        
        message.User.AccountWorldData = null;
        message.User.State = LoginState.SelectWorld;
        message.User.SelectedWorldID = null;
        message.User.SelectedChannelID = null;

        return Task.CompletedTask;
    }
}
