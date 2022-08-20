using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    private readonly IEvent<IUserEnterStage<TStageUser>> _enterEvent;
    private readonly IEvent<IUserLeaveStage<TStageUser>> _leaveEvent;
    private readonly ICollection<TStageUser> _users;

    protected AbstractStage(
        IEvent<IUserEnterStage<TStageUser>> enterEvent,
        IEvent<IUserLeaveStage<TStageUser>> leaveEvent
    )
    {
        _enterEvent = enterEvent;
        _leaveEvent = leaveEvent;
        _users = new List<TStageUser>();
    }

    public IReadOnlyCollection<TStageUser> Users => _users.ToImmutableList();

    public async Task Enter(TStageUser user)
    {
        user.Stage = this;
        _users.Add(user);

        await _enterEvent.Publish(new UserEnterStage<TStageUser>(user));
    }

    public async Task Leave(TStageUser user)
    {
        user.Stage = null;
        _users.Remove(user);

        await _leaveEvent.Publish(new UserLeaveStage<TStageUser>(user));
    }
}
