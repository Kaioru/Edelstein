using Edelstein.Protocol.Gameplay;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser> where TStageUser : IStageUser
{
    public abstract Task Enter(TStageUser user);
    public abstract Task Leave(TStageUser user);
}
