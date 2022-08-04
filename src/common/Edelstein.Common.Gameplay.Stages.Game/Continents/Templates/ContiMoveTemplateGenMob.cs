using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Continents.Templates;

public record ContiMoveTemplateGenMob : IContiMoveTemplateGenMob
{
    public ContiMoveTemplateGenMob(IDataProperty genMob)
    {
        ItemID = genMob.Resolve<int>("genMobItemID") ?? 0;
        Position = new Point2D(
            genMob?.Resolve<int>("genMobPosition_x") ?? 0,
            genMob?.Resolve<int>("genMobPosition_y") ?? 0
        );
    }

    public int ItemID { get; }
    public IPoint2D Position { get; }
}
