using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public short MaxPerSlot { get; set; }

        public override void Parse(int id, IDataProperty p)
        {
            base.Parse(id, p);

            MaxPerSlot = p.Resolve<short>("info/slotMax") ?? 100;
        }
    }
}