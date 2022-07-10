using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Login.Templates
{
    public record WorldTemplate : ITemplate
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public byte State { get; init; }
        public bool BlockCharCreation { get; init; }

        public WorldTemplate(int id, IDataProperty property)
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
            State = property.Resolve<byte>("state") ?? 0;
            BlockCharCreation = property.Resolve<bool>("blockCharCreation") ?? false;
        }
    }
}
