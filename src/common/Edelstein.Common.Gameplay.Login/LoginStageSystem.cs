using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystem(
    ILoginStageSystemOptions options,
    ILoginContextPipelines pipelines
) : AbstractStageSystem<ILoginStageUser, ILoginStageSystem>, ILoginStageSystem
{
    public override string ID => options.ID;

    protected override IPipeline<UserOnPacket<ILoginStageUser, ILoginStageSystem>> UserOnPacketPipeline => pipelines.UserOnPacketPipeline;
    protected override IPipeline<UserOnException<ILoginStageUser, ILoginStageSystem>> UserOnExceptionPipeline => pipelines.UserOnExceptionPipeline;
    protected override IPipeline<UserOnDisconnect<ILoginStageUser, ILoginStageSystem>> UserOnDisconnectPipeline => pipelines.UserOnDisconnectPipeline;
    
    public ILoginStageSystemOptions Options => options;
    public ILoginContextPipelines Pipelines => pipelines;

    public override ILoginStageUser CreateUser(ISocket socket)
        => new LoginStageUser(this, socket);
}
