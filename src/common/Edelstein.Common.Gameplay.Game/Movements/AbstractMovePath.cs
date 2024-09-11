using Edelstein.Protocol.Gameplay.Game.Movements;

namespace Edelstein.Common.Gameplay.Game.Movements;

public abstract class AbstractMovePath<TMoveAction> : IMovePath<TMoveAction> where TMoveAction : IMoveAction
{
    public TMoveAction? Action { get; set; }
    public short? Offset { get; set; }
    public short? X { get; set; }
    public short? Y { get; set; }
    public short? VX { get; set; }
    public short? VY { get; set; }
    public short? Fh { get; set; }
    public short? FhFallStart { get; set; }
    public short? XOffset { get; set; }
    public short? YOffset { get; set; }
    public bool? Stat { get; set; }
    
    public void Apply(StructuredMovePath path)
    {
        X = path.X;
        Y = path.Y;
        VX = path.VX;
        VY = path.VY;
        
        foreach (var f in path.Fragments)
        {
            if (f.Action != null) Action = GetActionFromValue(f.Action.Value);
            if (f.Offset != null) Offset = f.Offset;
            if (f.X != null) X = f.X;
            if (f.Y != null) Y = f.Y;
            if (f.VX != null) VX = f.VX;
            if (f.VY != null) VY = f.VY;
            if (f.Fh != null) Fh = f.Fh;
            if (f.FhFallStart != null) FhFallStart = f.FhFallStart;
            if (f.XOffset != null) XOffset = f.XOffset;
            if (f.YOffset != null) YOffset = f.YOffset;
            if (f.Stat != null) Stat = f.Stat;
        }
    }
    
    protected abstract TMoveAction GetActionFromValue(byte value);
}
