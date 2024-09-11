using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldLife<TMovePath, TMoveAction>(
    IPoint2D position,
    IFieldFoothold? foothold,
    TMoveAction action
) : AbstractFieldObject(position, foothold),
    IFieldLife<TMovePath, TMoveAction>
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    public TMoveAction Action { get; set; } = action;
    
    public async Task UpdatePosition(IFieldPortal portal)
    {
        if (Field == null) return;

        Position = portal.Position;
        Foothold = Field.Template.Footholds
            .Find(Position)
            .FirstOrDefault();
    }
    
    public async Task UpdatePosition(TMovePath path)
    {
        if (Field == null) return;
        
        if (path.Action != null) Action = path.Action;
        if (path.X != null && path.Y != null) Position = new Point2D(path.X.Value, path.Y.Value);
        if (path.Fh != null) Foothold = await Field.Template.Footholds.Retrieve(path.Fh.Value);
        
        await UpdateFieldSplit();
    }

    private async Task UpdateFieldSplit()
    {
        var split = Field?.GetSplit(Position);

        if (split == null && Field != null)
        {
            await Field.Enter(this);
            return;
        }

        if (split != null && FieldSplit != split)
            await split.Enter(this);
    }
}
