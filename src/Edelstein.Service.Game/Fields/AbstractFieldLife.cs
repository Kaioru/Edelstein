using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Movement;

namespace Edelstein.Service.Game.Fields
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public byte MoveAction { get; set; }
        public short Foothold { get; set; }

        public async Task Move(IPacket packet)
        {
            var movementPath = new MovementPath();
            movementPath.Decode(packet);
            Position = new Point(movementPath.X, movementPath.Y);
            MoveAction = movementPath.MoveActionLast;
            Foothold = movementPath.FHLast;
            await Field.BroadcastPacket(this, GetMovePacket(movementPath));
        }

        public abstract IPacket GetMovePacket(MovementPath path);
    }
}