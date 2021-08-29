using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates
{
    public record ItemStringTemplate : ITemplate
    {
        public int ID { get; }
        public string Name { get; }

        public ItemStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("name");
        }
    }
}
