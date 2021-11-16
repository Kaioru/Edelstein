using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public MoveActionType Action { get; set; }
        public IPhysicalLine2D Foothold { get; set; }

        public async Task Move(IMovePath path)
        {
            if (Field == null) return;
            if (path.Action.HasValue) Action = path.Action.Value;
            if (path.Position.HasValue) Position = path.Position.Value;
            if (path.FootholdID.HasValue) Foothold = Field.GetFoothold(path.FootholdID.Value);

            await UpdateFieldSplit();
        }
    }
}
