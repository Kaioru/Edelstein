using System;
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

            Foothold = path.FootholdID.HasValue ? Field.GetFoothold(path.FootholdID.Value) : null;

            Console.WriteLine($"Client FH: {path.FootholdID} (Pos: {path.Position}) : Server FH: {Field.GetFootholdUnderneath(path.Position.Value)}");

            await UpdateFieldSplit();
        }
    }
}
