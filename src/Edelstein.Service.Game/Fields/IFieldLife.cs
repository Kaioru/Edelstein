namespace Edelstein.Service.Game.Fields
{
    public interface IFieldLife : IFieldObj
    {
        byte MoveAction { get; set; }
        short Foothold { get; set; }
    }
}