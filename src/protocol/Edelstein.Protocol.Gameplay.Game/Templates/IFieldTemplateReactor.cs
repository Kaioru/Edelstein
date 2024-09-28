using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Templates;

public interface IFieldTemplateReactor
{
    string? Name { get; }
    int TemplateID { get; }
    
    int ReactorTime { get; }
    
    bool IsFacingLeft { get; }
    IPoint2D Position { get; }
}
