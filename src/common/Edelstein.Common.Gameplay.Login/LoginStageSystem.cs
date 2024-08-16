using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystem(
    LoginContext context
) : AbstractStageSystem<ILoginStageSystem, ILoginStageSystemUser>, ILoginStageSystem
{
    public override string ID => "TODO"; // TODO
    public LoginContext Context { get; } = context;

    protected override IPipeline<UserOnPacket<ILoginStageSystem, ILoginStageSystemUser>> OnPacketPipeline 
        => Context.Pipelines.UserOnPacket;
    protected override IPipeline<UserOnException<ILoginStageSystem, ILoginStageSystemUser>> OnExceptionPipeline 
        => Context.Pipelines.UserOnException;
    protected override IPipeline<UserOnDisconnect<ILoginStageSystem, ILoginStageSystemUser>> OnDisconnectPipeline 
        => Context.Pipelines.UserOnDisconnect;

    public override ILoginStageSystemUser Initialize(ISocket socket)
        => new LoginStageSystemUser(socket, this);
}
