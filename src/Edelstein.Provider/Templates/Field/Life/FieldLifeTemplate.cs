using System.Drawing;

namespace Edelstein.Provider.Templates.Field.Life
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

        public FieldLifeTemplate(IDataProperty property)
        {
            ID = property.Resolve<int>("id") ?? -1;
            Type = property.ResolveOrDefault<string>("type").ToLower() == "n"
                ? FieldLifeType.NPC
                : FieldLifeType.Monster;
            F = (byte) (property.Resolve<bool>("f") ?? false ? 0 : 1);
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