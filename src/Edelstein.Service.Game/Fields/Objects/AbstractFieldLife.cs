namespace Edelstein.Service.Game.Fields.Objects
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public byte MoveAction { get; set; }
        public short Foothold { get; set; }
    }
}