using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCTemplateScript : INPCTemplateScript
{
    public string Script { get; }
    
    public NPCTemplateScript(int id, IDataProperty property) =>
        Script = property.ResolveOrDefault<string>("script") ?? "NO-SCRIPT";
    // TODO: start end dates
}
