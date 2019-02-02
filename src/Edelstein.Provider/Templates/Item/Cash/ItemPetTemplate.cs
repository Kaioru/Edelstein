using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item.Cash
{
    public class ItemPetTemplate : ItemTemplate
    {
        public int Life { get; set; }

        public override void Parse(int id, IDataProperty p)
        {
            p.Resolve("info").Resolve(info =>
            {
                base.Parse(id, info);

                Life = info.Resolve<short>("life") ?? -1;
            });
        }
    }
}