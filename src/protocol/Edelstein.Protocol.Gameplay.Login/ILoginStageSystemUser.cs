namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystemUser : IStageSystemUser<ILoginStageSystem, ILoginStageSystemUser>
{
    public LoginState State { get; set; }
}
