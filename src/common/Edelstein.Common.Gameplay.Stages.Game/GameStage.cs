using Edelstein.Common.Gameplay.Stages.Contracts.Events;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStage : IGameStage
{
    private readonly IEvent<IUserEnterStage<IGameStageUser>> _enterEvent;
    private readonly IFieldManager _fieldManager;
    private readonly IEvent<IUserLeaveStage<IGameStageUser>> _leaveEvent;

    public GameStage(
        IFieldManager fieldManager,
        IEvent<IUserEnterStage<IGameStageUser>> enterEvent,
        IEvent<IUserLeaveStage<IGameStageUser>> leaveEvent
    )
    {
        _fieldManager = fieldManager;
        _enterEvent = enterEvent;
        _leaveEvent = leaveEvent;
    }

    public IReadOnlyCollection<IGameStageUser> Users => new List<IGameStageUser>();

    public async Task Enter(IGameStageUser user)
    {
        if (user.Character == null)
        {
            await user.Disconnect();
            return;
        }

        var field = await _fieldManager.Retrieve(user.Character.FieldID);
        var fieldUser = _fieldManager.CreateUser(user);

        if (field == null || fieldUser == null)
        {
            await user.Disconnect();
            return;
        }

        user.Stage = this;
        user.FieldUser = fieldUser;

        await field.Enter(fieldUser);
        await _enterEvent.Publish(new UserEnterStage<IGameStageUser>(user, this));

        _ = user.Dispatch(new PacketWriter() // temp fix to remove event titles
            .WriteShort(280)
            .WriteString("")
            .WriteByte(255)
            .WriteString("")
            .WriteByte(255)
            .WriteString("")
            .WriteByte(255)
            .WriteString("")
            .WriteByte(255)
            .WriteString("")
            .WriteByte(255));
    }

    public async Task Leave(IGameStageUser user)
    {
        user.Stage = null;

        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
        await _leaveEvent.Publish(new UserLeaveStage<IGameStageUser>(user, this));
    }

    public Task OnTick(DateTime now) => Task.CompletedTask;
}
