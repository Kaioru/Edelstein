using Edelstein.Protocol.Gameplay.Login.Contexts;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageUser : IStageUser<ILoginStageUser>
{
    LoginContext Context { get; }
    LoginState State { get; set; }

    byte? SelectedWorldID { get; set; }
    byte? SelectedChannelID { get; set; }
}

