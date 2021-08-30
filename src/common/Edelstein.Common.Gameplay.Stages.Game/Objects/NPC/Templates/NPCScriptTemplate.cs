using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates
{
    public record NPCScriptTemplate : ITemplate
    {
        public int ID { get; init; }

        public string Script { get; init; }

        public NPCScriptTemplate(int id, IDataProperty property)
        {
            ID = id;

            Script = property.ResolveOrDefault<string>("script");
            // TODO: start end dates
        }
    }
}
