namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldLife : IFieldObj
    {
        byte MoveAction { get; set; }
        short Foothold { get; set; }
    }
}