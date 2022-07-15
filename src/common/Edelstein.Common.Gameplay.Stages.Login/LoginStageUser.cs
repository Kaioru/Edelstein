using Edelstein.Common.Gameplay.Stages.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUser : AbstractStageUser, ILoginStageUser
{
    private readonly IPipeline<IStageUserOnDisconnect<ILoginStageUser>> _onDisconnect;
    private readonly IPipeline<IStageUserOnException<ILoginStageUser>> _onException;
    private readonly IPipeline<IStageUserOnPacket<ILoginStageUser>> _onPacket;

    public LoginStageUser(
        ISocket socket,
        IPipeline<IStageUserOnPacket<ILoginStageUser>> onPacket,
        IPipeline<IStageUserOnException<ILoginStageUser>> onException,
        IPipeline<IStageUserOnDisconnect<ILoginStageUser>> onDisconnect
    ) : base(socket)
    {
        _onPacket = onPacket;
        _onException = onException;
        _onDisconnect = onDisconnect;
    }

    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public override Task OnPacket(IPacketReader packet) =>
        _onPacket.Process(new StageUserOnPacket<ILoginStageUser>(this, packet));

    public override Task OnException(Exception exception) =>
        _onException.Process(new StageUserOnException<ILoginStageUser>(this, exception));

    public override Task OnDisconnect() =>
        _onDisconnect.Process(new StageUserOnDisconnect<ILoginStageUser>(this));
}
