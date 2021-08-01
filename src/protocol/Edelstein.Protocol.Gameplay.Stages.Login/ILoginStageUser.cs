namespace Edelstein.Protocol.Gameplay.Stages.Login
{
    public interface ILoginStageUser<TStage, TUser> : IServerStageUser<TStage, TUser>
        where TStage : ILoginStage<TStage, TUser>
        where TUser : ILoginStageUser<TStage, TUser>
    {
    }
}
