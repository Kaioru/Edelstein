using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractFieldHandler : IPacketHandler<IGameStageUser>
{
    public abstract short Operation { get; }

    public virtual bool Check(IGameStageUser user) => user.Field != null && user.FieldUser != null;

    public Task Handle(IGameStageUser user, IPacketReader reader) =>
        Handle(user.FieldUser!, reader);

    protected abstract Task Handle(IFieldUser user, IPacketReader reader);
}
