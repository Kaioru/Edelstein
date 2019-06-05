namespace Edelstein.Provider.Templates.Item.Cash
{
    public class PetItemTemplate : ItemTemplate
    {
        public int Life { get; set; }

        public PetItemTemplate(int id, IDataProperty info) : base(id, info)
        {
            Life = info.Resolve<short>("life") ?? -1;
        }
    }
}