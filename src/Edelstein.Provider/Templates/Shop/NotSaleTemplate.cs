namespace Edelstein.Provider.Templates.Shop
{
    public class NotSaleTemplate : ITemplate
    {
        public int ID { get; }

        public NotSaleTemplate(int id)
        {
            ID = id;
        }
    }
}