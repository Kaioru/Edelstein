namespace Edelstein.Provider.Templates.Item
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public double UnitPrice { get; set; }
        public short MaxPerSlot { get; set; }
        
        public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
        {
            UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
            MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
        }
    }
}