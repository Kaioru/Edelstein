using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Utilities.Pipelines;
using Injectio.Attributes;

namespace Edelstein.Common.Gameplay.Login.Contexts;

[RegisterScoped]
public class LoginContextPipelines(
    IPipeline<UserOnPacket<ILoginStageUser, ILoginStageSystem>> userOnPacketPipeline, 
    IPipeline<UserOnException<ILoginStageUser, ILoginStageSystem>> userOnExceptionPipeline,
    IPipeline<UserOnDisconnect<ILoginStageUser, ILoginStageSystem>> userOnDisconnectPipeline
) : ILoginContextPipelines
{
    public IPipeline<UserOnPacket<ILoginStageUser, ILoginStageSystem>> UserOnPacketPipeline { get; } = userOnPacketPipeline;
    public IPipeline<UserOnException<ILoginStageUser, ILoginStageSystem>> UserOnExceptionPipeline { get; } = userOnExceptionPipeline;
    public IPipeline<UserOnDisconnect<ILoginStageUser, ILoginStageSystem>> UserOnDisconnectPipeline { get; } = userOnDisconnectPipeline;
}
