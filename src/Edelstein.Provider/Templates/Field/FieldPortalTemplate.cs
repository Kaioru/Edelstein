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

        public static FieldPortalTemplate Parse(IDataProperty property)
        {
            var t = new FieldPortalTemplate();

            property.Resolve(p =>
            {
                t.ID = Convert.ToInt32(p.Name);
                t.Name = p.ResolveOrDefault<string>("pn");
                t.Type = (FieldPortalType) (p.Resolve<int>("pt") ?? 0);
                t.Script = p.ResolveOrDefault<string>("script");
                t.ToMap = p.Resolve<int>("tm") ?? int.MinValue;
                t.ToName = p.ResolveOrDefault<string>("tn");
                t.Position = new Point(
                    p.Resolve<int>("x") ?? int.MinValue,
                    p.Resolve<int>("y") ?? int.MinValue
                );
            });
            return t;
        }
    }
}