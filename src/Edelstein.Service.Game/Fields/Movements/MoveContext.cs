using System.Drawing;

namespace Edelstein.Service.Game.Fields.Movements
{
    public class MoveContext : IMoveContext
    {
        public byte? MoveAction { get; set; }
        public short? Foothold { get; set; }
        public Point? Position { get; set; }
    }
}