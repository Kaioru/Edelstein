using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUser : AbstractStageUser<ILoginStage, ILoginStageUser>, ILoginStageUser
{
    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public LoginStageUser(ISocket socket) : base(socket)
    {
    }
}
