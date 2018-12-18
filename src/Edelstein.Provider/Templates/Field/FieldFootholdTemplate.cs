using System;
using Edelstein.Provider.Parser;
using PKG1;

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

        public static FieldFootholdTemplate Parse(IDataProperty p)
        {
            var res = new FieldFootholdTemplate
            {
                ID = Convert.ToInt32(p.Name),
                Next = p.Resolve<int>("next") ?? 0,
                Prev = p.Resolve<int>("prev") ?? 0,
                X1 = p.Resolve<int>("x1") ?? 0,
                X2 = p.Resolve<int>("x2") ?? 0,
                Y1 = p.Resolve<int>("y1") ?? 0,
                Y2 = p.Resolve<int>("y2") ?? 0
            };

            return res;
        }
    }
}