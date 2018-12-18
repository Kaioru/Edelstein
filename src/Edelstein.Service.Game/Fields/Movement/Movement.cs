namespace Edelstein.Service.Game.Fields.Movement
{
    public class Movement
    {
        public byte Attr { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short VX { get; set; }
        public short VY { get; set; }
        
        public byte MoveAction { get; set; }
        
        public short FH { get; set; }
        public short FHFallStart { get; set; }
        public short Elapse { get; set; }
        
        public bool Stat { get; set; }
        
        public short XOffset { get; set; }
        public short YOffset { get; set; }
    }
}