namespace Edelstein.Protocol.Gameplay.Stages.Login;

public interface ILoginStageUser<TStage, TStageUser> : IStageUser<TStage, TStageUser>
    where TStage : ILoginStage<TStage, TStageUser>
    where TStageUser : ILoginStageUser<TStage, TStageUser>
{
    LoginState State { get; set; }

    byte? SelectedWorldID { get; set; }
    byte? SelectedChannelID { get; set; }
}
