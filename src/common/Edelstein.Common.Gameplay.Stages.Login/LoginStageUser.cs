using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUser : AbstractStageUser, ILoginStageUser
{
    public LoginStageUser(ISocket socket) : base(socket)
    {
    }

    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public override Task OnPacket(IPacket packet) => throw new NotImplementedException();

    public override Task OnException(Exception exception) => throw new NotImplementedException();

    public override Task OnDisconnect() => throw new NotImplementedException();
}
