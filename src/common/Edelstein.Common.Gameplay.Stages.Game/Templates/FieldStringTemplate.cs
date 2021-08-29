using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public record FieldStringTemplate : ITemplate
    {
        public int ID { get; }

        public string MapName { get; }
        public string StreetName { get; }

        public FieldStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            MapName = property.ResolveOrDefault<string>("mapName");
            StreetName = property.ResolveOrDefault<string>("streetName");
        }
    }
}
