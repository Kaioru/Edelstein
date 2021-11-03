namespace Edelstein.Protocol.Gameplay.Stages.ITC
{
    public interface IITCStageUser<TStage, TUser> : IServerStageUser<TStage, TUser>
        where TStage : IITCStage<TStage, TUser>
        where TUser : IITCStageUser<TStage, TUser>
    {
    }
}
