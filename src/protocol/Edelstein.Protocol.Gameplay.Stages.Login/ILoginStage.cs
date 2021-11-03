namespace Edelstein.Protocol.Gameplay.Stages.Login
{
    public interface ILoginStage<TStage, TUser> : IServerStage<TStage, TUser>
        where TStage : ILoginStage<TStage, TUser>
        where TUser : ILoginStageUser<TStage, TUser>
    {
    }
}
