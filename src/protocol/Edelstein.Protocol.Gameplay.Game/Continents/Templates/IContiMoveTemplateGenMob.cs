using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Continents.Templates;

public interface IContiMoveTemplateGenMob
{
    int ItemID { get; }
    IPoint2D Position { get; }
}
