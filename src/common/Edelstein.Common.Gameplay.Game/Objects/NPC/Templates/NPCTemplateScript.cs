using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCTemplateScript : INPCTemplateScript
{

    public NPCTemplateScript(int id, IDataNode node) =>
        Script = node.ResolveString("script") ?? "NO-SCRIPT";

    public string Script { get; }
    // TODO: start end dates
}
