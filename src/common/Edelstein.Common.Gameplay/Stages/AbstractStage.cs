using Edelstein.Protocol.Gameplay.Stages;

namespace Edelstein.Common.Gameplay.Stages;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    public Task Enter(TStageUser user)
    {
        user.Stage = this;
        return Task.CompletedTask;
    }

    public Task Leave(TStageUser user)
    {
        user.Stage = null;
        return Task.CompletedTask;
    }
}
