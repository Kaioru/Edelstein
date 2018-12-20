namespace Edelstein.Service.Game.Field
{
    public interface IFieldLife : IFieldObj
    {
        byte MoveAction { get; set; }
        short Foothold { get; set; }
    }
}