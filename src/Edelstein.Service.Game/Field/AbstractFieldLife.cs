using System.Drawing;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Field.Movement;

namespace Edelstein.Service.Game.Field
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public byte MoveAction { get; set; }
        public short Foothold { get; set; }

        public MovementPath Move(IPacket packet)
        {
            var movementPath = new MovementPath();
            movementPath.Decode(packet);
            Position = new Point(movementPath.X, movementPath.Y);
            MoveAction = movementPath.MoveActionLast;
            Foothold = movementPath.FHLast;
            return movementPath;
        }
    }
}