namespace Edelstein.Provider.Templates.Item.Cash
{
    public class ItemPetTemplate : ItemTemplate
    {
        public int Life { get; set; }

        public ItemPetTemplate(int id, IDataProperty info) : base(id, info)
        {
            Life = info.Resolve<short>("life") ?? -1;
        }
    }
}