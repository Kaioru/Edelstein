using System;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldFootholdTemplate : ITemplate
    {
        public int ID { get; }
        public int Next { get; }
        public int Prev { get; }
        public int X1 { get; }
        public int X2 { get; }
        public int Y1 { get; }
        public int Y2 { get; }

        public FieldFootholdTemplate(IDataProperty property)
        {
            ID = Convert.ToInt32(property.Name);
            Next = property.Resolve<int>("next") ?? 0;
            Prev = property.Resolve<int>("prev") ?? 0;
            X1 = property.Resolve<int>("x1") ?? 0;
            X2 = property.Resolve<int>("x2") ?? 0;
            Y1 = property.Resolve<int>("y1") ?? 0;
            Y2 = property.Resolve<int>("y2") ?? 0;
        }
    }
}