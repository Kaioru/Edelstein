using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.NotSale
{
    public class NotSaleTemplate : ITemplate
    {
        public int ID { get; set; }

        public static NotSaleTemplate Parse(int id, IDataProperty property)
        {
            var t = new NotSaleTemplate
            {
                ID = id
            };

            return t;
        }
    }
}