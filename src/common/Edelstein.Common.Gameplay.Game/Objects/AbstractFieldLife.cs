using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Spatial.Collections;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldLife(
    IPoint2D position,
    IFieldFoothold? foothold = null
) : AbstractFieldObject(position, foothold), IFieldLife
{
    public async Task UpdatePosition(IFieldPortal portal)
    {
        if (Field == null) return;

        Position = portal.Position;
        Foothold = Field.Template.Footholds
            .Find(Position)
            .FirstOrDefault();
    }
    
    public async Task UpdatePosition()
    {
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
