using System;

namespace Edelstein.Provider.Templates.NPC
{
    public class NPCScriptTemplate : ITemplate
    {
        public int ID { get; set; }

        public string Script { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public NPCScriptTemplate(IDataProperty property)
        {
            ID = Convert.ToInt32(property.Name);

            Script = property.ResolveOrDefault<string>("script");
            Start = property.Resolve<int>("start") ?? 0;
            End = property.Resolve<int>("end") ?? 0;
        }
    }
}