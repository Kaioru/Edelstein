using System;
using System.Drawing;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldPortalTemplate : ITemplate
    {
        public int ID { get; set; }
        public FieldPortalType Type { get; set; }

        public string Name { get; set; }
        public string Script { get; set; }

        public int ToMap { get; set; }
        public string ToName { get; set; }

        public Point Position { get; set; }

        public FieldPortalTemplate(IDataProperty property)
        {
            ID = Convert.ToInt32(property.Name);
            Name = property.ResolveOrDefault<string>("pn");
            Type = (FieldPortalType) (property.Resolve<int>("pt") ?? 0);
            Script = property.ResolveOrDefault<string>("script");
            ToMap = property.Resolve<int>("tm") ?? int.MinValue;
            ToName = property.ResolveOrDefault<string>("tn");
            Position = new Point(
                property.Resolve<int>("x") ?? int.MinValue,
                property.Resolve<int>("y") ?? int.MinValue
            );
        }
    }
}