using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldLife : AbstractFieldObject, IFieldLife
{
    protected AbstractFieldLife(IPoint2D position) : base(position)
    {
    }

    public IFieldFoothold? Foothold { get; private set; }

    public Task Move(IPoint2D position)
    {
        Position = position;
        Foothold = Field?.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));
        return Task.CompletedTask;
    }
}
