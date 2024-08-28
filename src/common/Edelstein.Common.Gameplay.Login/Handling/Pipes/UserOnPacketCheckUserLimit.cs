using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckUserLimit : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckUserLimit>>
{
    public Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckUserLimit> message)
        => message.User.Dispatch(new CheckUserLimitResult());
}
