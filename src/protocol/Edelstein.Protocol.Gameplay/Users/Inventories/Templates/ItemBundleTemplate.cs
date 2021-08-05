using Edelstein.Protocol.Parser;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates
{
    public record ItemBundleTemplate : ItemTemplate
    {
        public double UnitPrice { get; init; }
        public short MaxPerSlot { get; init; }

        public ItemBundleTemplate(int id, IDataProperty info) : base(id, info)
        {
            UnitPrice = info.Resolve<double>("unitPrice") ?? 0.0;
            MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
        }
    }
}
