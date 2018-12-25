using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public double UnitPrice { get; set; }
        public short MaxPerSlot { get; set; }

        public override void Parse(int id, IDataProperty p)
        {
            p.Resolve("info").Resolve(info =>
            {
                base.Parse(id, info);

                UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
                MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
            });
        }
    }
}