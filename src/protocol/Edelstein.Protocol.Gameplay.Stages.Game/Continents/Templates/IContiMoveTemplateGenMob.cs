using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;

public interface IContiMoveTemplateGenMob
{
    int ItemID { get; }
    IPoint2D Position { get; }
}
