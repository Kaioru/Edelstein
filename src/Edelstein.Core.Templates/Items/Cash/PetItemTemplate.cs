using Edelstein.Provider;

namespace Edelstein.Core.Templates.Items.Cash
{
    public class PetItemTemplate : ItemTemplate
    {
        public int Life { get; }

        public PetItemTemplate(int id, IDataProperty info) : base(id, info)
        {
            Life = info.Resolve<short>("life") ?? -1;
        }
    }
}