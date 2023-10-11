using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Continents.Templates;

public record ContiMoveTemplateGenMob : IContiMoveTemplateGenMob
{
    public ContiMoveTemplateGenMob(IDataNode genMob)
    {
        ItemID = genMob.ResolveInt("genMobItemID") ?? 0;
        Position = new Point2D(
            genMob.ResolveInt("genMobPosition_x") ?? 0,
            genMob.ResolveInt("genMobPosition_y") ?? 0
        );
    }

    public int ItemID { get; }
    public IPoint2D Position { get; }
}
