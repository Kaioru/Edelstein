namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates
{
    public record ItemBundleTemplate : ItemTemplate
    {
        public double UnitPrice { get; init; }
        public short MaxPerSlot { get; init; }
    }
}
