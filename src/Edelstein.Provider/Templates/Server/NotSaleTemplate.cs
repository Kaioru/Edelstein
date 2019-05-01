namespace Edelstein.Provider.Templates.Server
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