using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldLife<TMovePath, TMoveAction> :
    AbstractFieldObject, IFieldLife<TMovePath, TMoveAction>
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    public TMoveAction Action { get; protected set; }
    public IFieldFoothold? Foothold { get; private set; }

    protected AbstractFieldLife(TMoveAction action, IPoint2D position, IFieldFoothold? foothold = null) : base(position)
    {
        Action = action;
        Foothold = foothold;
    }
    
    public Task Move(IPoint2D position)
    {
        Position = position;
        Foothold = Field?.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));
        return UpdateFieldSplit();
    }

    public async Task Move(TMovePath ctx)
    {
        if (Field == null) return;

        if (ctx.Action != null) Action = ctx.Action;
        if (ctx.Position != null) Position = ctx.Position;

        Foothold = Field.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));

        await UpdateFieldSplit();

        if (FieldSplit != null)
            await FieldSplit.Dispatch(GetMovePacket(ctx), this);
    }

    private async Task UpdateFieldSplit()
    {
        var split = Field?.GetSplit(Position);

        if (split == null)
        {
            if (Field != null) await Field.Enter(this);
            return;
        }

        if (FieldSplit != split) await split.Enter(this);
    }

    protected abstract IPacket GetMovePacket(TMovePath ctx);
}
