using System;
using System.Drawing;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldPortalTemplate : ITemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public FieldPortalType Type { get; set; }

        public string Script { get; set; }

        public int ToMap { get; set; }
        public string ToName { get; set; }

        public Point Position { get; set; }

        public static FieldPortalTemplate Parse(IDataProperty p)
        {
            var res = new FieldPortalTemplate
            {
                ID = Convert.ToInt32(p.Name),
                Name = p.ResolveOrDefault<string>("pn"),
                Type = (FieldPortalType) (p.Resolve<int>("pt") ?? 0),
                Script = p.ResolveOrDefault<string>("script"),
                ToMap = p.Resolve<int>("tm") ?? int.MinValue,
                ToName = p.ResolveOrDefault<string>("tn"),
                Position = new Point(
                    p.Resolve<int>("x") ?? int.MinValue,
                    p.Resolve<int>("y") ?? int.MinValue
                )
            };

            return res;
        }
    }
}