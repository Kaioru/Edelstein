using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Items
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public double UnitPrice { get; }
        public short MaxPerSlot { get; }

        public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
        {
            UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
            MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
        }
    }
}