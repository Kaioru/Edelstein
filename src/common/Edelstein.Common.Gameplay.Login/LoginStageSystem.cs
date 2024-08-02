using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystem(
    LoginContext context
) : AbstractStageSystem<ILoginStageSystemUser, ILoginStageSystem>, ILoginStageSystem
{
    public override string ID => "TODO"; // TODO
    public LoginContext Context { get; } = context;

    protected override IPipeline<UserOnPacket<ILoginStageSystemUser, ILoginStageSystem>> OnPacketPipeline 
        => Context.Pipelines.UserOnPacket;
    protected override IPipeline<UserOnException<ILoginStageSystemUser, ILoginStageSystem>> OnExceptionPipeline 
        => Context.Pipelines.UserOnException;
    protected override IPipeline<UserOnDisconnect<ILoginStageSystemUser, ILoginStageSystem>> OnDisconnectPipeline 
        => Context.Pipelines.UserOnDisconnect;

    public override ILoginStageSystemUser Initialize(ISocket socket)
        => new LoginStageSystemUser(socket, this);
}
