using System.Drawing;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Fields.Life
{
    public class FieldLifeTemplate
    {
        public int TemplateID { get; }
        public FieldLifeType Type { get; }

        public int MobTime { get; }

        public bool Left { get; }
        public Point Position { get; }
        public int RX0 { get; }
        public int RX1 { get; }
        public int FH { get; }

        public FieldLifeTemplate(IDataProperty property)
        {
            TemplateID = property.Resolve<int>("id") ?? -1;
            Type = property.ResolveOrDefault<string>("type").ToLower() == "n"
                ? FieldLifeType.NPC
                : FieldLifeType.Monster;

            MobTime = property.Resolve<int>("mobTime") ?? 0;

            Left = !(property.Resolve<bool>("f") ?? false);
            Position = new Point(
                property.Resolve<int>("x") ?? int.MinValue,
                property.Resolve<int>("y") ?? int.MinValue
            );
            RX0 = property.Resolve<int>("rx0") ?? int.MinValue;
            RX1 = property.Resolve<int>("rx1") ?? int.MinValue;
            FH = property.Resolve<int>("fh") ?? 0;
        }
    }
}