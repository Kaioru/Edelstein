namespace Edelstein.Protocol.Gameplay.Stages.Login;

public interface ILoginStage<TStage, TStageUser> : IStage<TStage, TStageUser>
    where TStage : ILoginStage<TStage, TStageUser>
    where TStageUser : ILoginStageUser<TStage, TStageUser>
{
}
