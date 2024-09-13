using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Templates;

public interface IFieldTemplateLife
{
    FieldLifeType Type { get; }

    int TemplateID { get; }
    
    int MobTime { get; }

    bool IsFacingLeft { get; }
    
    IPoint2D Position { get; }
    IRect2D Bounds { get; }
    
    int Foothold { get; }
}
