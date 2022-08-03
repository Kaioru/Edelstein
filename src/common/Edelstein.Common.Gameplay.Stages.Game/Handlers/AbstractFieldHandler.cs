using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers;

public abstract class AbstractFieldHandler : IPacketHandler<IGameStageUser>
{
    public abstract short Operation { get; }

    public virtual bool Check(IGameStageUser user) => user.Field != null && user.FieldUser != null;

    public Task Handle(IGameStageUser user, IPacketReader reader) =>
        Handle(user.FieldUser!, reader);

    protected abstract Task Handle(IFieldUser user, IPacketReader reader);
}
