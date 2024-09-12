namespace Edelstein.Protocol.Gameplay.Game.Movements;

public interface IMovePath<TMoveAction> where TMoveAction : IMoveAction
{
    TMoveAction? Action { get; set; }
    short? Offset { get; set; }
    
    short? X { get; set; }
    short? Y { get; set; }
    
    short? VX { get; set; }
    short? VY { get; set; }
    
    short? Fh { get; set; }
    short? FhFallStart { get; set; }
    
    short? XOffset { get; set; }
    short? YOffset { get; set; }
    
    byte? Stat { get; set; }

    void Apply(StructuredMovePath path);
}
