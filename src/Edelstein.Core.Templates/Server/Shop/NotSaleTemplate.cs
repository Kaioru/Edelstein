using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class NotSaleTemplate : IDataTemplate
    {
        public int ID { get; }

        public NotSaleTemplate(int id)
        {
            ID = id;
        }
    }
}