using System;

namespace Edelstein.Provider.Templates.Field.Life.NPC
{
    public class NPCScriptTemplate : ITemplate
    {
        public int ID { get; }

        public string Script { get; }
        public int Start { get; }
        public int End { get; }

        public NPCScriptTemplate(IDataProperty property)
        {
            ID = Convert.ToInt32(property.Name);

            Script = property.ResolveOrDefault<string>("script");
            Start = property.Resolve<int>("start") ?? 0;
            End = property.Resolve<int>("end") ?? 0;
        }
    }
}