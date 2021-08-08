using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public MoveActionType Action { get; set; }
        public short Foothold { get; set; }

        // TODO
        public Task Move(IMovePath path) { throw new System.NotImplementedException(); }
    }
}
