using System.Drawing;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldReactorTemplate : ITemplate
    {
        public int ID { get; set; }

        public bool F { get; set; }
        public Point Position { get; set; }

        public static FieldReactorTemplate Parse(IDataProperty property)
        {
            var t = new FieldReactorTemplate();

            property.Resolve(p =>
            {
                t.ID = p.Resolve<int>("id") ?? -1;
                t.F = p.Resolve<bool>("f") ?? false;
                t.Position = new Point(
                    p.Resolve<int>("x") ?? int.MinValue,
                    p.Resolve<int>("y") ?? int.MinValue
                );
            });
            return t;
        }
    }
}