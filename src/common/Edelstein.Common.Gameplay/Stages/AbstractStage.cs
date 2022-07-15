using Edelstein.Protocol.Gameplay.Stages;

namespace Edelstein.Common.Gameplay.Stages;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser> where TStageUser : IStageUser
{
    public abstract Task Enter(TStageUser user);
    public abstract Task Leave(TStageUser user);
}
