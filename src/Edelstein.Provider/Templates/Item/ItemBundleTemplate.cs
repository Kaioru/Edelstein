using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public short MaxPerSlot { get; set; }

        public override void Parse(int id, IDataProperty p)
        {
            var info = p.Resolve("info");
            
            base.Parse(id, info);

            MaxPerSlot = info.Resolve<short>("slotMax") ?? 100;
        }
    }
}