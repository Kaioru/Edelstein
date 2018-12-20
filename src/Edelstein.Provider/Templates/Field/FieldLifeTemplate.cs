using System.Drawing;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldLifeTemplate : ITemplate
    {
        public int ID { get; set; }
        public FieldLifeType Type { get; set; }

        public byte F { get; set; }
        public Point Position { get; set; }
        public int RX0 { get; set; }
        public int RX1 { get; set; }
        public int FH { get; set; }

        public static FieldLifeTemplate Parse(IDataProperty property)
        {
            var t = new FieldLifeTemplate();

            property.Resolve(p =>
            {
                t.ID = p.Resolve<int>("id") ?? -1;
                t.Type = p.ResolveOrDefault<string>("type").ToLower() == "n"
                    ? FieldLifeType.NPC
                    : FieldLifeType.Monster;
                t.F = (byte) (p.Resolve<bool>("f") ?? false ? 0 : 1);
                t.Position = new Point(
                    p.Resolve<int>("x") ?? int.MinValue,
                    p.Resolve<int>("y") ?? int.MinValue
                );
                t.RX0 = p.Resolve<int>("rx0") ?? int.MinValue;
                t.RX1 = p.Resolve<int>("rx1") ?? int.MinValue;
                t.FH = p.Resolve<int>("fh") ?? 0;
            });
            return t;
        }
    }
}