using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates
{
    public record ItemBundleTemplate : ItemTemplate
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
