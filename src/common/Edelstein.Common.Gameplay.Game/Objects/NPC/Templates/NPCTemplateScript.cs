using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCTemplateScript : INPCTemplateScript
{
    public string Script { get; }
    // TODO: start end dates
    
    public NPCTemplateScript(IDataNode node) 
        => Script = node.ResolveString("script") ?? "NO-SCRIPT";
}
