using System;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldFootholdTemplate : ITemplate
    {
        public int ID { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public static FieldFootholdTemplate Parse(IDataProperty property)
        {
            var t = new FieldFootholdTemplate();

            property.Resolve(p =>
            {
                t.ID = Convert.ToInt32(p.Name);
                t.Next = p.Resolve<int>("next") ?? 0;
                t.Prev = p.Resolve<int>("prev") ?? 0;
                t.X1 = p.Resolve<int>("x1") ?? 0;
                t.X2 = p.Resolve<int>("x2") ?? 0;
                t.Y1 = p.Resolve<int>("y1") ?? 0;
                t.Y2 = p.Resolve<int>("y2") ?? 0;
            });
            return t;
        }
    }
}