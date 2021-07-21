using Edelstein.Protocol.Parser;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates
{
    public record ItemPetTemplate : ItemTemplate
    {
        public int Life { get; init; }

        public ItemPetTemplate(int id, IDataProperty info) : base(id, info)
        {
            Life = info.Resolve<short>("life") ?? -1;
        }
    }
}
