namespace Edelstein.Protocol.Gameplay.Stages.Login
{
    public interface ILoginStageUser<TStage, TUser> : IMigrateableStageUser<TStage, TUser>
        where TStage : ILoginStage<TStage, TUser>
        where TUser : ILoginStageUser<TStage, TUser>
    {
    }
}
