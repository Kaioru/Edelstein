using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.NPC
{
    public class NPCScriptTemplate
    {
        public string Script { get; }
        public int Start { get; }
        public int End { get; }

        public NPCScriptTemplate(IDataProperty property)
        {
            Script = property.ResolveOrDefault<string>("script");
            Start = property.Resolve<int>("start") ?? 0;
            End = property.Resolve<int>("end") ?? 0;
        }
    }
}