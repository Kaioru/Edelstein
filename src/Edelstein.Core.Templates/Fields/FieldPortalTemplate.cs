using System.Drawing;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Fields
{
    public class FieldPortalTemplate
    {
        public FieldPortalType Type { get; }

        public string Name { get; }
        public string Script { get; }

        public int ToMap { get; }
        public string ToName { get; }

        public Point Position { get; }

        public FieldPortalTemplate(IDataProperty property)
        {
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