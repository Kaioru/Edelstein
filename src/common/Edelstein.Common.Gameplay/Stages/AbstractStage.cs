using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages;

namespace Edelstein.Common.Gameplay.Stages;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    private readonly ICollection<TStageUser> _users;

    protected AbstractStage() => _users = new List<TStageUser>();

    public IReadOnlyCollection<TStageUser> Users => _users.ToImmutableList();

    public Task Enter(TStageUser user)
    {
        user.Stage = this;
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task Leave(TStageUser user)
    {
        user.Stage = null;
        _users.Remove(user);
        return Task.CompletedTask;
    }
}
