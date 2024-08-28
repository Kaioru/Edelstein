namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystemUser : IStageSystemUser<ILoginStageSystem, ILoginStageSystemUser>
{
    LoginState State { get; set; }
    
    byte? SelectedWorldID { get; set; }
    byte? SelectedChannelID { get; set; }
}
