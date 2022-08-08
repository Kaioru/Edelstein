using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldTemplateLife : IIdentifiable<int>
{
    FieldLifeType Type { get; }

    int MobTime { get; }

    bool IsFacingLeft { get; }
    IPoint2D Position { get; }
    IRectangle2D Bounds { get; }
    int FootholdID { get; }
}
