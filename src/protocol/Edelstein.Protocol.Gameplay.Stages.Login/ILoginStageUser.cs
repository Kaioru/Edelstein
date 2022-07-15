using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Login;

public interface ILoginStageUser : IStageUser
{
    ILoginContext Context { get; }

    LoginState State { get; set; }

    byte? SelectedWorldID { get; set; }
    byte? SelectedChannelID { get; set; }
}
