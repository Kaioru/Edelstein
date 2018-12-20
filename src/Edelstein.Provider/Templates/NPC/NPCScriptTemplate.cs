using System;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.NPC
{
    public class NPCScriptTemplate : ITemplate
    {
        public int ID { get; set; }
        
        public string Script { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public static NPCScriptTemplate Parse(IDataProperty property)
        {
            var t = new NPCScriptTemplate();

            property.Resolve(s =>
            {
                t.ID = Convert.ToInt32(s.Name);

                t.Script = s.ResolveOrDefault<string>("script");
                t.Start = s.Resolve<int>("start") ?? 0;
                t.End = s.Resolve<int>("end") ?? 0;
            });
            return t;
        }
    }
}