namespace Edelstein.Protocol.Gameplay.Stages.ITC
{
    public interface IITCStage<TStage, TUser> : IServerStage<TStage, TUser>
        where TStage : IITCStage<TStage, TUser>
        where TUser : IITCStageUser<TStage, TUser>
    {
    }
}
