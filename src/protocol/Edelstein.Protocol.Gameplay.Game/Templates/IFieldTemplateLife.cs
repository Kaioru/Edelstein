using Edelstein.Protocol.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Templates;

public interface IFieldTemplateLife : IIdentifiable<int>
{
    FieldLifeType Type { get; }

    int MobTime { get; }

    bool IsFacingLeft { get; }
    IPoint2D Position { get; }
    IRectangle2D Bounds { get; }
    int FootholdID { get; }
}
