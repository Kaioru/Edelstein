using System.Drawing;

namespace Edelstein.Service.Game.Fields.Movements
{
    public interface IMoveContext
    {
        byte? MoveAction { get; set; }
        short? Foothold { get; set; }
        Point? Position { get; set; }
    }
}