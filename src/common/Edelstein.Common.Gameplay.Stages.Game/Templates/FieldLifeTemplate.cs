using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public class FieldLifeTemplate : ITemplate
    {
        public int ID { get; }

        public FieldLifeType Type { get; }
        public int TemplateID { get; }

        public int MobTime { get; }

        public bool Left { get; }
        public Point2D Position { get; }
        public int RX0 { get; }
        public int RX1 { get; }
        public int FH { get; }

        public FieldLifeTemplate(int id, IDataProperty property)
        {
            ID = id;

            Type = property.ResolveOrDefault<string>("type").ToLower() == "n"
                ? FieldLifeType.NPC
                : FieldLifeType.Monster;
            TemplateID = property.Resolve<int>("id") ?? -1;

            MobTime = property.Resolve<int>("mobTime") ?? 0;

            Left = !(property.Resolve<bool>("f") ?? false);
            Position = new Point2D(
                property.Resolve<int>("x") ?? int.MinValue,
                property.Resolve<int>("y") ?? int.MinValue
            );
            RX0 = property.Resolve<int>("rx0") ?? int.MinValue;
            RX1 = property.Resolve<int>("rx1") ?? int.MinValue;
            FH = property.Resolve<int>("fh") ?? 0;
        }
    }
}
