using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public record FieldStringTemplate : ITemplate
    {
        public int ID { get; set; }

        public string MapName { get; set; }
        public string StreetName { get; set; }

        public FieldStringTemplate(int id, IDataProperty property)
        {
            ID = id;

            MapName = property.ResolveOrDefault<string>("mapName");
            StreetName = property.ResolveOrDefault<string>("streetName");
        }
    }
}
