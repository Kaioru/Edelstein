using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldLife : AbstractFieldObject, IFieldLife
{
    protected AbstractFieldLife(IPoint2D position) : base(position)
    {
    }

    public byte Action { get; protected set; }
    public IFieldFoothold? Foothold { get; private set; }

    public void SetPosition(IPoint2D position)
    {
        Position = position;
        Foothold = Field?.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));
    }

    public Task Move(IPoint2D position) =>
        Move(position, Field?.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position)));

    public async Task Move(IMovePath ctx)
    {
        if (Field == null) return;

        if (ctx.Action != null) Action = ctx.Action.Value;
        await Move(ctx.Position, ctx.Foothold.HasValue
            ? Field.Template.Footholds.FindByID(ctx.Foothold.Value)
            : null
        );

        if (FieldSplit != null)
            await FieldSplit.Dispatch(GetMovePacket(ctx), this);
    }

    private async Task Move(IPoint2D? position, IFieldFoothold? foothold)
    {
        if (position != null) Position = position;
        Foothold = foothold;

        var split = Field?.GetSplit(Position);

        if (split == null)
        {
            if (Field != null) await Field.Enter(this);
            return;
        }

        if (FieldSplit != split) await split.Enter(this);
    }

    protected abstract IPacket GetMovePacket(IMovePath ctx);
}
