using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent.Templates
{
    public record ContiMoveGenMobTemplate : ITemplate
    {
        public int ID { get; init; }

        public int TemplateID { get; init; }
        public Point2D Position { get; init; }
    }
}
