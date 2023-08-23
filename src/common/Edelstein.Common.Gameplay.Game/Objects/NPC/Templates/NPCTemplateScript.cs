using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCTemplateScript : INPCTemplateScript
{

    public NPCTemplateScript(int id, IDataProperty property) =>
        Script = property.ResolveOrDefault<string>("script") ?? "NO-SCRIPT";

    public string Script { get; }
    // TODO: start end dates
}
