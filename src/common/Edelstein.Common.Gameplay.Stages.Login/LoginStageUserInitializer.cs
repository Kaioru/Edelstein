using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUserInitializer : IAdapterInitializer
{
    private readonly IPipeline<IStageUserOnDisconnect<ILoginStageUser>> _onDisconnect;
    private readonly IPipeline<IStageUserOnException<ILoginStageUser>> _onException;
    private readonly IPipeline<IStageUserOnPacket<ILoginStageUser>> _onPacket;

    public LoginStageUserInitializer(
        IPipeline<IStageUserOnPacket<ILoginStageUser>> onPacket,
        IPipeline<IStageUserOnException<ILoginStageUser>> onException,
        IPipeline<IStageUserOnDisconnect<ILoginStageUser>> onDisconnect
    )
    {
        _onPacket = onPacket;
        _onException = onException;
        _onDisconnect = onDisconnect;
    }

    public IAdapter Initialize(ISocket socket) => new LoginStageUser(socket, _onPacket, _onException, _onDisconnect);
}
