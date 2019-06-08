using System.Drawing;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Field.Reactor
{
    public class FieldReactorTemplate : ITemplate
    {
        public int ID { get; }

        public int ReactorTime { get; }
        public bool F { get; }
        public Point Position { get; }

        public FieldReactorTemplate(IDataProperty property)
        {
            ID = property.Resolve<int>("id") ?? -1;

            ReactorTime = property.Resolve<int>("reactorTime") ?? 0;

            F = property.Resolve<bool>("f") ?? false;
            Position = new Point(
                property.Resolve<int>("x") ?? int.MinValue,
                property.Resolve<int>("y") ?? int.MinValue
            );
        }
    }
}