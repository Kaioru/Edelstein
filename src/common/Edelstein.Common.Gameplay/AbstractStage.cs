using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStage<TStage, TStageUser> : IStage<TStage, TStageUser>
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
    private readonly IDictionary<int, TStageUser> _users;

    public abstract string ID { get; }

    protected AbstractStage()
        => _users = new Dictionary<int, TStageUser>();

    public virtual Task Enter(TStageUser user)
    {
        _users.Add(user.ID, user);
        return Task.CompletedTask;
    }

    public virtual Task Leave(TStageUser user)
    {
        _users.Remove(user.ID);
        return Task.CompletedTask;
    }

    public IEnumerable<TStageUser> GetUsers() => _users.Values.ToImmutableList();
}
